namespace TutorApplication.ApplicationCore.SignalR.Persistence
{
	public class MessageUsers
	{
		public List<OnlineUser> Users = new List<OnlineUser>();

		public Task AddNewOnlineUser(OnlineUser user)
		{
			lock (Users)
			{
				var index = Users.FindIndex(u=>u.UserId == user.UserId);
				if(index >= 0)
				{
					Users[index] = user;
				}
				else
				{
					Users.Add(user);
				}
				
			
				return Task.CompletedTask;
			}
		}

		public Task RemoveNewOnlineUser(string userName)
		{
			lock (Users)
			{
				var user = Users.RemoveAll(u => u.UserName == userName);
				
				return Task.CompletedTask;
			}
		}


	}



}
