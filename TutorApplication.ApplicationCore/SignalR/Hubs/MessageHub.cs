using CloudinaryDotNet;
using Microsoft.AspNetCore.SignalR;
using TutorApplication.ApplicationCore.SignalR.Persistence;
using TutorApplication.ApplicationCore.SignalR.Services;
using TutorApplication.ApplicationCore.Utils;
using TutorApplication.Infrastructure.Data;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Extensions;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses.Messages;
using static TutorApplication.ApplicationCore.Services.MessageService;

namespace TutorApplication.ApplicationCore.SignalR.Hubs
{
	public class MessageHub : Hub
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMessageHubServices _messageHubServices;
		private readonly IHubServices _hubServices;
		private readonly IHubContext<PresenceHub> _presenceHub;
		private readonly OnlineUsers _onlineUsers;
		private readonly MessageUsers _messageUsers;
		private readonly ApplicationDbContext _db;

		public MessageHub(IUnitOfWork unitOfWork, IMessageHubServices messageHubServices, 
			IHubServices hubServices, IHubContext<PresenceHub> presenceHub, 
			OnlineUsers onlineUsers, MessageUsers messageUsers,
			ApplicationDbContext db)
		{
			_unitOfWork = unitOfWork;
			_messageHubServices = messageHubServices;
			_hubServices = hubServices;
			_presenceHub = presenceHub;
			_onlineUsers = onlineUsers;
			_messageUsers = messageUsers;
			_db = db;

		}

		public SessionInfo GroupState { get; set; }

		public override async Task OnConnectedAsync()
		{
			//await ConnectToGroup();
		}

		public async Task ChangeGroup(SessionInfo sessionInfo)
		{
			GroupState = sessionInfo;
			//await ConnectToGroup(usingContextHttp: false);

		}

		public async Task ConnectToGroup(SessionInfo session)
		{
			try
			{
				session.ConnectionId = Context.ConnectionId;
				session.UserId = Context.User.GetUserId();
				await _onlineUsers.AddSession(session);
				string recieverName = await _hubServices.GetReceiver(session.IsGroup, session.RecieverId, session.CourseGroupId);
				var groupName = HubUtils.GetGroupName(session.SenderEmail, recieverName, session.IsGroup);
				await _hubServices.AddConnectionToGroup(groupName, Context.ConnectionId, Context.User.GetUserEmail());
				await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
				var onlineUser = new OnlineUser
				{
					GroupName = groupName,
					ConnectionId = Context.ConnectionId,
					UserName = Context.User.GetUserEmail()
				};
				await _messageUsers.AddNewOnlineUser(onlineUser);

				//Enables the messaging to take place
				if (session.IsGroup == "false")
				{
					await _messageHubServices.AddUserGroupDirect(groupName, Context.User.GetUserEmail(), Context.User.GetUserId(), session.RecieverId, session.CourseGroupId, session.IsGroup);

				}
				else
				{
					await _messageHubServices.AddUserGroup(groupName, Context.User.GetUserEmail(), Context.User.GetUserId(), session.RecieverId, session.CourseGroupId, session.IsGroup);

				}



				List<MessageResponse> responses;
				if (session.IsGroup == "true")
				{
					var missedMessages = await _unitOfWork.MissedMessage.GetItems(u => u.RecieverId == Context.User.GetUserId() && u.isGroup == "true");
					var groupMessages = missedMessages.Where(u => u.CourseGroupId == session.CourseGroupId);
					await _unitOfWork.MissedMessage.DeleteItems(groupMessages);
					await _unitOfWork.SaveChanges();

					responses = await _messageHubServices.GetCourseGroupMessages(session.CourseGroupId, session.SenderId);
				}
				else
				{
					var missedMessages = await _unitOfWork.MissedMessage.GetItems(u => u.RecieverId == Context.User.GetUserId() && u.isGroup == "false");
					var directMessages = missedMessages.Where(u => u.SenderId == session.RecieverId);
					await _unitOfWork.MissedMessage.DeleteItems(directMessages);
					await _unitOfWork.SaveChanges();

					responses = await _messageHubServices.GetDirectMessages(session.RecieverId, session.SenderId);
				}

				await Clients.Client(Context.ConnectionId).SendAsync("GetMessages", responses);
			}
			catch (Exception ex)
			{
				throw new HubException("Yes!!!");
			}
		}


		public async Task SendMessage(MessageRequest request, SessionInfo session)
		{
			var senderEmail = Context.User.GetUserEmail();
			var senderId = Context.User.GetUserId();
			var isGroup = request.isGroup.ToString().ToLower();

			string recieverName = await _hubServices.GetReceiver(isGroup, (Guid)request.RecieverId, (Guid)request.CourseGroupId);

			
			var courseGroupId = session.CourseGroupId;
		
			//await _db.MissedMessages.AddAsync(message);
			//await _db.SaveChangesAsync();
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

			await SendMessageAlert(groupName, senderId,courseGroupId, isGroup);
		}

		public async Task MessageTyping(bool isTyping,SessionInfo session)
		{
			if (session == null) throw new HubException();
			string recieverName = await _hubServices.GetReceiver(session.IsGroup, session.RecieverId, session.CourseGroupId);
			var groupName = HubUtils.GetGroupName(session.SenderEmail, recieverName, session.IsGroup);

			var connections = await _unitOfWork.Connections.GetItems(u => u.GroupName == groupName);

			var messageConnections = await _unitOfWork.Connections.GetItems(u => u.GroupName == groupName && u.Username != Context.User.GetUserEmail());
			var messageConnectionsIds = messageConnections.Select(u => u.ConnectionURL).ToList();

			await Clients.Clients(messageConnectionsIds).SendAsync("IsTyping", isTyping);
		}


		public async Task SendMessageAlert(string groupName,Guid senderId, Guid courseGroupId,string isGroup)
		{
			//Gets Active Users

			var activeUsers = _messageUsers.Users.Select(u => u.UserName);
			var messageUsersActive = _messageUsers.Users.Where(u=>u.GroupName==groupName).Select(u => u.UserName).ToList();

			//Gets Users that are not active in the current chat

			var userGroups = await _unitOfWork.UserGroups.GetItems(u => u.GroupName == groupName && u.UserName != Context.User.GetUserEmail() && !messageUsersActive.Contains(u.UserName));
			var connectionUsernames = userGroups.Select(u => u.UserName).ToList();



			//Used For Missed Messages
			var m = await _unitOfWork.UserGroups.GetItems(u => u.GroupName == groupName );
			var mlist = m.ToList();
			var missedCallGroups = await _unitOfWork.UserGroups.GetItems(u => u.GroupName == groupName && u.UserName != Context.User.GetUserEmail() && !activeUsers.Contains(u.UserName));
			var connectionUserIds = missedCallGroups.Select(u => u.UserId).ToList();
			var missedMessages =  connectionUserIds.Select(u => new MissedMessage()
			{
				SenderId = senderId,
				RecieverId = (Guid) u,
				CourseGroupId = courseGroupId,
				isGroup = isGroup
			}).ToList();
			await _unitOfWork.MissedMessage.AddItems(missedMessages);
			await _unitOfWork.SaveChanges();

			//Gets Users that are not active, but onliine
			AlertMessage alertMessage = new();
			if (isGroup == "true")
			{
				
				var course = await _unitOfWork.Courses.GetItem(u => u.Id == courseGroupId);
				alertMessage.NavigationId = course.NavigationId;
				alertMessage.Name = course.CourseTitle;
			}
			else
			{
				var user = await _unitOfWork.Users.GetItem(u => u.Id == senderId);
				alertMessage.NavigationId = user.NavigationId;
				alertMessage.Name = user.LastName+" "+user.FirstName;
			}
			
			

			var onlineConnectionIds = _onlineUsers.Users.Where(u => connectionUsernames.Contains(u.UserName)).Select(u => u.ConnectionId).ToList();
			await _presenceHub.Clients.Clients(onlineConnectionIds).SendAsync("NewMessageAlert", alertMessage);
		}

		public class AlertMessage
		{
			public Guid NavigationId { get; set; }
			public string Name { get; set; }
		}
		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			//await DisconnetGroup();
			var session = _onlineUsers.Sessions.Where(u => u.ConnectionId == Context.ConnectionId).FirstOrDefault();
			if(session != null)
			{
				await DisconnectFromGroup(session);
			}
			
		}


		public async Task DisconnectGroup(SessionInfo session)
		{
			GroupState = session;
			//await DisconnectGroup(false);
		}
		public async Task DisconnectFromGroup(SessionInfo session)
		{
			//var session = GetSessionInfo(usingContextHttp);
			var initialUserGroup = new UserGroup();
			if (session.IsGroup == "false")
			{
				initialUserGroup = await _unitOfWork.UserGroups.GetItem(u => u.isGroup == session.IsGroup && u.UserId == session.SenderId && u.RecieverId == session.RecieverId);

			}
			else
			{
				initialUserGroup = await _unitOfWork.UserGroups.GetItem(u => u.isGroup == session.IsGroup && u.UserId == session.SenderId && u.RecieverId == session.RecieverId && u.CourseGroupId == session.CourseGroupId);

			}
			var groupName = initialUserGroup.GroupName;

			var user = await _unitOfWork.Users.GetItem(u => u.Id == session.SenderId);
			user.LastSeen = DateTime.UtcNow;
			await _unitOfWork.SaveChanges();
			await _messageUsers.RemoveNewOnlineUser(Context.User.GetUserEmail());



			if (initialUserGroup != null)
			{
				initialUserGroup.LastSeen = DateTime.UtcNow;
			}

			var amount = await _unitOfWork.SaveChanges();

			await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

			
			await _hubServices.RemoveConnection(Context.User.GetUserEmail());
			if (session!=null)
			{
				await MessageTyping(false, session);
			}
			
			
		}

		private SessionInfo GetSessionInfo(bool usingContextHttp = true)
		{
			if (usingContextHttp)
			{
				var httpContext = Context.GetHttpContext();
				string? CourseGroupId = httpContext?.Request.Query["CourseGroupId"];
				string? RecieverId = httpContext?.Request.Query["RecieverId"];
				string? isGroup = httpContext?.Request.Query["isGroup"];

				if (RecieverId == null) throw new HubException("No User");

				var senderEmail = Context.User.GetUserEmail();
				var senderId = Context.User.GetUserId();
				GroupState = new SessionInfo() { IsGroup = isGroup, RecieverId = Guid.Parse(RecieverId), SenderEmail = senderEmail, SenderId = senderId, CourseGroupId = Guid.Parse(CourseGroupId) };

			}

			return GroupState;
		}



	}

}
