using TutorApplication.ApplicationCore.Extensions;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses.Messages;

namespace TutorApplication.ApplicationCore.SignalR.Services
{
	public class MessageHubServices : IMessageHubServices
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMessageService _messageService;

		public MessageHubServices(IUnitOfWork unitOfWork, IMessageService messageService)
		{
			_unitOfWork = unitOfWork;
			_messageService = messageService;
		}

		public async Task<MessageResponse> SendDirectMessage(MessageRequest request, Guid senderId, string groupName)
		{
			var directMessage = new DirectMessageRequest()
			{
				Content = request.Content,
				RecieverId = (Guid)request.RecieverId,
				Photos = request.Photos
			};
			var res = await _messageService.SendDirectMessage(directMessage, senderId);
			var phots = new Photo();

			return new MessageResponse()
			{
				Id = res.Id,
				SenderId = senderId,
				Content = res.Content,
				Created = res.Created,
				Photos = res.Photos!=null? res.Photos.ConvertPhotoToPhotoResponse():null,
				SenderName = res.Sender.LastName + " " + res.Sender.FirstName
			};
		}

		public async Task AddUserGroup(string groupName, string userName,Guid userId,Guid recieverId, Guid courseGroupId,string isGroup )
		{
			
			var userGroup = await _unitOfWork.UserGroups.GetItems(u => 
			u.UserName == userName && 
			u.UserId == userId &&
			u.RecieverId == recieverId &&
			u.CourseGroupId == courseGroupId &&
			u.GroupName == groupName&&
			u.isGroup == isGroup);

			if (userGroup.Count() == 0)
			{
				await _unitOfWork.UserGroups.AddItem(new UserGroup() { 
					GroupName = groupName, 
					UserName = userName,
					UserId=userId ,
					RecieverId = recieverId,
					CourseGroupId = courseGroupId,
					isGroup = isGroup
				});
				await _unitOfWork.SaveChanges();
			}
		}
		public async Task AddUserGroupDirect(string groupName, string userName, Guid userId, Guid recieverId, Guid courseGroupId, string isGroup)
		{

			var userGroup = await _unitOfWork.UserGroups.GetItems(u =>
			u.UserId == userId &&
			u.RecieverId == recieverId &&
			u.isGroup == isGroup);

			if (userGroup.Count() == 0)
			{
				await _unitOfWork.UserGroups.AddItem(new UserGroup()
				{
					GroupName = groupName,
					UserName = userName,
					UserId = userId,
					RecieverId = recieverId,
					CourseGroupId = courseGroupId,
					isGroup = isGroup
				});
				await _unitOfWork.SaveChanges();
			}
		}
		public async Task<MessageResponse> SendGroupMessage(MessageRequest request, Guid senderId, string groupName)
		{
			var courseGroupMessage = new CourseGroupMessageRequest()
			{
				Content = request.Content,
				CourseGroupId = (Guid)request.CourseGroupId,
				Photos = request.Photos
			};
			var res = await _messageService.SendCourseGroupMessage(courseGroupMessage, senderId);
			var course = await _unitOfWork.Courses.GetItem(u => u.Id == request.CourseGroupId);
			return new MessageResponse()
			{
				Id = res.Id,
				SenderId = senderId,
				Content = res.Content,
				Created = res.Created,
				Photos = res.Photos != null ? res.Photos.ConvertPhotoToPhotoResponse() : null,
				SenderName = res.Sender.LastName + " "+ res.Sender.FirstName + (course.TutorId == senderId ? " (Tutor)" : "")
			};
		}

		public async Task<List<MessageResponse>> GetCourseGroupMessages(Guid courseGroupId, Guid senderId)
		{
			var responses = await _messageService.GetCourseGroupMessage(courseGroupId, senderId);

			return responses;
		}

		public async Task<List<MessageResponse>> GetDirectMessages(Guid recieverId, Guid senderId)
		{
			var responses = await _messageService.GetDirectMessages(recieverId, senderId);

			return responses;
		}




	}

	public interface IMessageHubServices
	{
		Task<MessageResponse> SendGroupMessage(MessageRequest request, Guid senderId, string groupName);
		Task<MessageResponse> SendDirectMessage(MessageRequest request, Guid senderId, string groupName);
		Task<List<MessageResponse>> GetCourseGroupMessages(Guid courseGroupId, Guid senderId);
		Task<List<MessageResponse>> GetDirectMessages(Guid recieverId, Guid senderId);
		Task AddUserGroup(string groupName, string userName, Guid userId, Guid recieverId, Guid courseGroupId, string isGroup);
		Task AddUserGroupDirect(string groupName, string userName, Guid userId, Guid recieverId, Guid courseGroupId, string isGroup);


	}


}
