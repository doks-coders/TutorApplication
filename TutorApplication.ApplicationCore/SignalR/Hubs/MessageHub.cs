using Azure.Core;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApplication.ApplicationCore.Services.Interfaces;
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

		public MessageHub(IUnitOfWork unitOfWork, IMessageService messageService, IMessageHubServices messageHubServices, IHubServices hubServices)
		{
			_unitOfWork = unitOfWork;
			_messageHubServices = messageHubServices;
			_hubServices = hubServices;

		}

	
		public override async Task OnConnectedAsync()
		{

			try
			{
				var session = GetSessionInfo();
				string recieverName= await _hubServices.GetReceiver(session.IsGroup,session.RecieverId,session.CourseGroupId);

				var groupName = HubUtils.GetGroupName(session.SenderEmail, recieverName, session.IsGroup);
				await _hubServices.AddConnectionToGroup(groupName, Context.ConnectionId);
				await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
				List<MessageResponse> responses;
				if (session.IsGroup == "true")
				{
					responses = await _messageHubServices.GetCourseGroupMessages(int.Parse(session.CourseGroupId), session.SenderId);
				}
				else
				{
					responses = await _messageHubServices.GetDirectMessages(int.Parse(session.RecieverId), session.SenderId);
				}

				await Clients.Group(groupName).SendAsync("Connected", responses);
			}
			catch(Exception ex)
			{
				throw new HubException("Yes!!!");
			}
			
		}

		public async Task SendMessage(MessageRequest request)
		{
			var senderEmail = Context.User.GetUserEmail();
			var senderId = Context.User.GetUserId();
			var isGroup = request.isGroup.ToString().ToLower();

			string recieverName = await _hubServices.GetReceiver(isGroup, request.RecieverId.ToString(), request.CourseGroupId.ToString());

			var groupName =  HubUtils.GetGroupName(senderEmail, recieverName, isGroup);

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
		}


		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			var groupName = await _hubServices.GetGroupWithConnectionId(Context.ConnectionId);

			await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
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

			return new SessionInfo() { IsGroup=isGroup,RecieverId=RecieverId,SenderEmail=senderEmail,SenderId=senderId,CourseGroupId=CourseGroupId};
		}

		
		
	}
	
}
