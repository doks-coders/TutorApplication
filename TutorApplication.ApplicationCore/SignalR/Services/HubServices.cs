using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;

namespace TutorApplication.ApplicationCore.SignalR.Services
{
	public class HubServices:IHubServices
	{
		private readonly IUnitOfWork _unitOfWork;
	
		public HubServices(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
	
		}
		public async Task<string> GetGroupWithConnectionId(string connectionId)
		{
			var connection = await _unitOfWork.Connections.GetItem(u => u.ConnectionURL == connectionId);
			if (connection == null) return string.Empty;
			return connection.GroupName;
		}

		public async Task AddConnectionToGroup(string groupName, string connectionId)
		{
			Group? group = await _unitOfWork.Groups.GetItem(e => e.Name == groupName);
			if (group == null)
			{
				await _unitOfWork.Groups.AddItem(new Group { Name = groupName });
			}
			else
			{
				var item = new Connection() { ConnectionURL = connectionId, GroupName = groupName, GroupId = group.Id };
				await _unitOfWork.Connections.AddItem(item);
			}
			await _unitOfWork.SaveChanges();

		}


		public async Task<string> GetReceiver(string IsGroup, string RecieverId, string CourseGroupId)
		{

			string recieverName = "";
			if (IsGroup == "false")
			{
				var reciever = await _unitOfWork.Users.GetItem(u => u.Id == int.Parse(RecieverId));
				if (reciever != null)
				{
					recieverName = reciever.Email;
				}
			}
			else
			{
				var reciever = await _unitOfWork.Courses.GetItem(u => u.Id == int.Parse(CourseGroupId));
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
		Task AddConnectionToGroup(string groupName, string connectionId);
		Task<string> GetReceiver(string IsGroup, string RecieverId, string CourseGroupId);

	}
}
