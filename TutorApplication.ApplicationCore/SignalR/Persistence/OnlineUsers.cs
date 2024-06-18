namespace TutorApplication.ApplicationCore.SignalR.Persistence
{
	public class OnlineUsers
	{
		public List<OnlineUser> Users = new List<OnlineUser>();

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
		public string ConnectionId { get; set; }
	}
}
