using Microsoft.AspNetCore.SignalR;
using TutorApplication.ApplicationCore.SignalR.Persistence;
using TutorApplication.ApplicationCore.SignalR.Services;
using TutorApplication.ApplicationCore.Utils;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Extensions;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses.Messages;

namespace TutorApplication.ApplicationCore.SignalR.Hubs
{
	public class MessageHub : Hub
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMessageHubServices _messageHubServices;
		private readonly IHubServices _hubServices;
		private readonly IHubContext<PresenceHub> _presenceHub;
		private readonly OnlineUsers _onlineUsers;

		public MessageHub(IUnitOfWork unitOfWork, IMessageHubServices messageHubServices, IHubServices hubServices, IHubContext<PresenceHub> presenceHub, OnlineUsers onlineUsers)
		{
			_unitOfWork = unitOfWork;
			_messageHubServices = messageHubServices;
			_hubServices = hubServices;
			_presenceHub = presenceHub;
			_onlineUsers = onlineUsers;

		}


		public override async Task OnConnectedAsync()
		{

			try
			{
				var session = GetSessionInfo();
				string recieverName = await _hubServices.GetReceiver(session.IsGroup, session.RecieverId, session.CourseGroupId);

				var groupName = HubUtils.GetGroupName(session.SenderEmail, recieverName, session.IsGroup);
				await _hubServices.AddConnectionToGroup(groupName, Context.ConnectionId, Context.User.GetUserEmail());
				await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
				await _messageHubServices.AddUserGroup(groupName, Context.User.GetUserEmail());
				List<MessageResponse> responses;
				if (session.IsGroup == "true")
				{
					responses = await _messageHubServices.GetCourseGroupMessages(session.CourseGroupId, session.SenderId);
				}
				else
				{
					responses = await _messageHubServices.GetDirectMessages(session.RecieverId, session.SenderId);
				}

				await Clients.Group(groupName).SendAsync("Connected", responses);
			}
			catch (Exception ex)
			{
				throw new HubException("Yes!!!");
			}

		}


		public async Task SendMessage(MessageRequest request)
		{
			var senderEmail = Context.User.GetUserEmail();
			var senderId = Context.User.GetUserId();
			var isGroup = request.isGroup.ToString().ToLower();

			string recieverName = await _hubServices.GetReceiver(isGroup, (Guid)request.RecieverId, (Guid)request.CourseGroupId);

			var groupName = HubUtils.GetGroupName(senderEmail, recieverName, isGroup);

			MessageResponse response;
			if (request.isGroup)
			{
				response = await _messageHubServices.SendGroupMessage(request, senderId, groupName);
			}
			else
			{
				response = await _messageHubServices.SendDirectMessage(request, senderId, groupName);
			}
			await Clients.Group(groupName).SendAsync("NewMessage", response);

			await SendMessageAlert(groupName);
		}

		public async Task MessageTyping(bool isTyping)
		{
			var session = GetSessionInfo();
			string recieverName = await _hubServices.GetReceiver(session.IsGroup, session.RecieverId, session.CourseGroupId);
			var groupName = HubUtils.GetGroupName(session.SenderEmail, recieverName, session.IsGroup);

			var messageConnections = await _unitOfWork.Connections.GetItems(u => u.GroupName == groupName && u.Username != Context.User.GetUserEmail());
			var messageConnectionsIds = messageConnections.Select(u => u.ConnectionURL).ToList();
			await Clients.Clients(messageConnectionsIds).SendAsync("IsTyping", isTyping);
		}
		public async Task SendMessageAlert(string groupName)
		{
			//Gets Active Users
			var messageConnections = await _unitOfWork.Connections.GetItems(u => u.GroupName == groupName);
			var messageUsersActive = messageConnections.Select(u => u.Username).ToList();

			//Gets Users that are not active but online
			var userGroups = await _unitOfWork.UserGroups.GetItems(u => u.GroupName == groupName && u.UserName != Context.User.GetUserEmail() && !messageUsersActive.Contains(u.UserName));
			var connectionUsernames = userGroups.Select(u => u.UserName).ToList();

			//Sends response to them
			var onlineConnectionIds = _onlineUsers.Users.Where(u => connectionUsernames.Contains(u.UserName)).Select(u => u.ConnectionId).ToList();
			await _presenceHub.Clients.Clients(onlineConnectionIds).SendAsync("NewMessageAlert", "New Message");
		}
		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			var groupName = await _hubServices.GetGroupWithConnectionId(Context.ConnectionId);

			await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
			await _hubServices.RemoveConnection(Context.User.GetUserEmail());
			await MessageTyping(false);
		}

		private SessionInfo GetSessionInfo()
		{
			var httpContext = Context.GetHttpContext();
			string? CourseGroupId = httpContext?.Request.Query["CourseGroupId"];
			string? RecieverId = httpContext?.Request.Query["RecieverId"];
			string? isGroup = httpContext?.Request.Query["isGroup"];

			if (RecieverId == null) throw new HubException("No User");

			var senderEmail = Context.User.GetUserEmail();
			var senderId = Context.User.GetUserId();

			return new SessionInfo() { IsGroup = isGroup, RecieverId = Guid.Parse(RecieverId), SenderEmail = senderEmail, SenderId = senderId, CourseGroupId = Guid.Parse(CourseGroupId) };
		}



	}

}
