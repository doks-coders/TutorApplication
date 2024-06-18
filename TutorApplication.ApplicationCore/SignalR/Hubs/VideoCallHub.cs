using Microsoft.AspNetCore.SignalR;
using TutorApplication.ApplicationCore.SignalR.Persistence;
using TutorApplication.ApplicationCore.SignalR.Services;
using TutorApplication.ApplicationCore.Utils;
using TutorApplication.SharedModels.Extensions;
using TutorApplication.SharedModels.Models;

namespace TutorApplication.ApplicationCore.SignalR.Hubs
{
	public class VideoCallHub : Hub
	{
		private readonly VideoGroupChats _videoGroupChats;
		private readonly IHubServices _hubServices;

		public VideoCallHub(VideoGroupChats videoGroupChats, IHubServices hubServices)
		{
			_videoGroupChats = videoGroupChats;
			_hubServices = hubServices;
		}

		public override async Task OnConnectedAsync()
		{
			var session = GetSessionInfo();
			string recieverName = await _hubServices.GetReceiver(session.IsGroup, session.RecieverId, session.CourseGroupId);

			var groupName = GetVideoGroupName(session.SenderEmail, recieverName, session.IsGroup);

			await _hubServices.AddConnectionToGroup(groupName, Context.ConnectionId, Context.User.GetUserEmail());
			await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

			var isActive = await _videoGroupChats.IsGroupVideoCallActive(groupName);
			await Clients.Group(groupName).SendAsync("IsGroupVideoCallActive", isActive);
		}

		public async Task StartVideoCallGroup(VideoState request)
		{
			var session = GetSessionInfo();



			string recieverName = await _hubServices.GetReceiver(session.IsGroup, session.RecieverId, session.CourseGroupId);
			var groupName = GetVideoGroupName(session.SenderEmail, recieverName, session.IsGroup);
			await _videoGroupChats.CreateVideoGroup(groupName, request);
			var groupMembers = await _videoGroupChats.GetVideoGroup(groupName);
			await Clients.Client(Context.ConnectionId).SendAsync("GetGroupMembers", groupMembers);
			await Clients.Group(groupName).SendAsync("IsGroupVideoCallActive", true);


		}


		public async Task EndVideoCallGroup()
		{
			var session = GetSessionInfo();
			string recieverName = await _hubServices.GetReceiver(session.IsGroup, session.RecieverId, session.CourseGroupId);
			var groupName = GetVideoGroupName(session.SenderEmail, recieverName, session.IsGroup);
			await _videoGroupChats.RemoveVideoGroup(groupName);
			var groupMembers = await _videoGroupChats.GetVideoGroup(groupName);
			await Clients.Group(groupName).SendAsync("GetGroupMembers", groupMembers ?? new List<VideoState>());
			await Clients.Group(groupName).SendAsync("IsGroupVideoCallActive", false);

		}


		public async Task JoinVideoCallGroup(VideoState request)
		{
			var session = GetSessionInfo();

			string recieverName = await _hubServices.GetReceiver(session.IsGroup, session.RecieverId, session.CourseGroupId);
			var groupName = GetVideoGroupName(session.SenderEmail, recieverName, session.IsGroup);
			await _videoGroupChats.JoinVideoGroup(groupName, request);
			var groupMembers = await _videoGroupChats.GetVideoGroup(groupName);
			await Clients.Group(groupName).SendAsync("NewGroupMember", request);

			await Clients.Client(Context.ConnectionId).SendAsync("GetGroupMembers", groupMembers);

		}

		public async Task SendNewMemberInfo(VideoState request)
		{
			var session = GetSessionInfo();
			string recieverName = await _hubServices.GetReceiver(session.IsGroup, session.RecieverId, session.CourseGroupId);
			var groupName = GetVideoGroupName(session.SenderEmail, recieverName, session.IsGroup);
			await Clients.Group(groupName).SendAsync("NewGroupMember", request);
		}

		//Might Have to optimiae later
		public async Task SendCommand(CommandPassed command)
		{
			var session = GetSessionInfo();
			string recieverName = await _hubServices.GetReceiver(session.IsGroup, session.RecieverId, session.CourseGroupId);
			var groupName = GetVideoGroupName(session.SenderEmail, recieverName, session.IsGroup);
			await _videoGroupChats.CommandAction(groupName, command);
			await Clients.Group(groupName).SendAsync("CommandPassed", command);
		}

		public async Task LeaveVideoGroup()
		{
			var session = GetSessionInfo();
			string elementId = session.SenderId.ToString();
			var groupName = await _hubServices.GetGroupWithConnectionId(Context.ConnectionId);
			await _videoGroupChats.LeaveVideoGroup(groupName, elementId);
			await Clients.Group(groupName).SendAsync("UserLeft", elementId);
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			var session = GetSessionInfo();
			string elementId = session.SenderId.ToString();
			var groupName = await _hubServices.GetGroupWithConnectionId(Context.ConnectionId);
			await _videoGroupChats.LeaveVideoGroup(groupName, elementId);
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
			await Clients.Group(groupName).SendAsync("UserLeft", elementId);
		}

		private string GetVideoGroupName(string senderEmail, string recieverName, string isGroup)
		{
			return HubUtils.GetGroupName(senderEmail, recieverName, isGroup) + "videogroup";
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
