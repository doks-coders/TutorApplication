using Microsoft.AspNetCore.SignalR;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.ApplicationCore.SignalR.Persistence;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Constants;
using TutorApplication.SharedModels.Extensions;
using TutorApplication.SharedModels.Responses.Messages;

namespace TutorApplication.ApplicationCore.SignalR.Hubs
{
	public class PresenceHub : Hub
	{
		private readonly OnlineUsers _onlineUsers;
		private readonly IMessageService _messageService;
		private readonly IUnitOfWork _unitOfWork;


		public PresenceHub(OnlineUsers onlineUsers, IUnitOfWork unitOfWork, IMessageService messageService)
		{
			_onlineUsers = onlineUsers;
			_unitOfWork = unitOfWork;
			_messageService = messageService;
		}

		public override async Task OnConnectedAsync()
		{
			var onlineUser = new OnlineUser { ConnectionId = Context.ConnectionId, UserName = Context.User.GetUserEmail() };
			await _onlineUsers.AddNewOnlineUser(onlineUser);
			await GetUpdatedDisplayContacts(true);

		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			await _onlineUsers.RemoveNewOnlineUser(Context.User.GetUserEmail());
			await GetUpdatedDisplayContacts(true);
		}

		public async Task GetUpdatedDisplayContacts(bool updateOtherusers)
		{

			Guid userId = Context.User.GetUserId();
			var role = Context.User.GetRole();
			var displayContacts = new List<DisplayMessageContact>();
			if (role == RoleConstants.Student)
			{
				displayContacts = await _messageService.GetContactsForStudentsWithHub(userId);
			}
			else
			{
				displayContacts = await _messageService.GetContactsForTutorswithHub(userId);
			}

			_onlineUsers.Users.ForEach(val =>
			{
				var index = displayContacts.FindIndex(u => u.Email == val.UserName);
				if (index != -1)
				{
					displayContacts[index].IsOnline = true;
				}
			});


			await Clients.Client(Context.ConnectionId).SendAsync("UsersOnline", displayContacts);

			if (updateOtherusers)
			{
				var emails = displayContacts.Select(e => e.Email);
				//Get my contacts from all displayContacts
				var contactIds = _onlineUsers.Users.Where(u => emails.Contains(u.UserName)).Select(e => e.ConnectionId);

				await Clients.Clients(contactIds).SendAsync("Refresh", displayContacts);
			}


		}





	}
}
