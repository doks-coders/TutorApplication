using TutorApplication.SharedModels.Models;

namespace TutorApplication.ApplicationCore.SignalR.Persistence
{
	public class OnlineUsers
	{
		public List<OnlineUser> Users = new List<OnlineUser>();

		public List<SessionInfo> Sessions = new List<SessionInfo>();

		public Task AddSession(SessionInfo session)
		{
			lock (session)
			{
				var index = Sessions.FindIndex(u => u.UserId == session.UserId);

				if(index >= 0)
				{
					Sessions[index] = session;
				}
				else
				{
					Sessions.Add(session);
				}
					
				
				
				
				return Task.CompletedTask;
			}
		}

		public Task AddNewOnlineUser(OnlineUser user)
		{
			lock (Users)
			{
				Users.Add(user);
				return Task.CompletedTask;
			}
		}

		public Task RemoveNewOnlineUser(string userName)
		{
			lock (Users)
			{
				Users.RemoveAll(u => u.UserName == userName); ;
				return Task.CompletedTask;
			}
		}


	}

	public class OnlineUser
	{
		public string UserName { get; set; }
		public Guid UserId { get; set; }

		public string GroupName { get; set; }
		public string ConnectionId { get; set; }
	}
}
