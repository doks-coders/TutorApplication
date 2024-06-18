namespace TutorApplication.ApplicationCore.SignalR.Persistence
{
	public class MessageUsers
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
				var user = Users.Where(u => u.UserName == userName).FirstOrDefault();
				Users.Remove(user);
				return Task.CompletedTask;
			}
		}


	}



}
