using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;

namespace TutorApplication.ApplicationCore.SignalR.Services
{
	public class HubServices : IHubServices
	{
		private readonly IUnitOfWork _unitOfWork;

		public HubServices(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public async Task RemoveConnection(string userName)
		{
			var connection = await _unitOfWork.Connections.GetItems(u => u.Username == userName);
			await _unitOfWork.Connections.DeleteItems(connection);
			await _unitOfWork.SaveChanges();

		}
		public async Task<string> GetGroupWithConnectionId(string connectionId)
		{
			var connection = await _unitOfWork.Connections.GetItem(u => u.ConnectionURL == connectionId);
			if (connection == null) return string.Empty;
			return connection.GroupName;
		}

		public async Task AddConnectionToGroup(string groupName, string connectionId, string userName)
		{
			Group? group = await _unitOfWork.Groups.GetItem(e => e.Name == groupName);
			if (group == null)
			{
				var newGroup = new Group { Name = groupName };
				await _unitOfWork.Groups.AddItem(newGroup);
				await _unitOfWork.SaveChanges();

				var item = new Connection() { ConnectionURL = connectionId, GroupName = groupName, GroupId = newGroup.Id, Username = userName };
				await _unitOfWork.Connections.AddItem(item);
			}
			else
			{
				var item = new Connection() { ConnectionURL = connectionId, GroupName = groupName, GroupId = group.Id, Username = userName };
				await _unitOfWork.Connections.AddItem(item);
			}
			await _unitOfWork.SaveChanges();

		}


		public async Task<string> GetReceiver(string IsGroup, Guid RecieverId, Guid CourseGroupId)
		{

			string recieverName = "";
			if (IsGroup == "false")
			{
				var reciever = await _unitOfWork.Users.GetItem(u => u.Id == RecieverId);
				if (reciever != null)
				{
					recieverName = reciever.Email;
				}
			}
			else
			{
				var reciever = await _unitOfWork.Courses.GetItem(u => u.Id == CourseGroupId);
				if (reciever != null)
				{
					recieverName = reciever.CourseTitle;
				}
			}
			return recieverName;
		}

	}

	public interface IHubServices
	{
		Task<string> GetGroupWithConnectionId(string connectionId);
		Task AddConnectionToGroup(string groupName, string connectionId, string userName);
		Task<string> GetReceiver(string IsGroup, Guid RecieverId, Guid CourseGroupId);
		Task RemoveConnection(string userName);

	}
}
