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
				RecieverId = (Guid)request.RecieverId
			};
			var res = await _messageService.SendDirectMessage(directMessage, senderId);

			return new MessageResponse()
			{
				Id = res.Id,
				SenderId = senderId,
				Content = res.Content,
				Created = res.Created
			};
		}

		public async Task AddUserGroup(string groupName, string userName)
		{
			var userGroup = await _unitOfWork.UserGroups.GetItems(u => u.UserName == userName && u.GroupName == groupName);
			if (userGroup.Count() == 0)
			{
				await _unitOfWork.UserGroups.AddItem(new UserGroup() { GroupName = groupName, UserName = userName });
				await _unitOfWork.SaveChanges();
			}
		}

		public async Task<MessageResponse> SendGroupMessage(MessageRequest request, Guid senderId, string groupName)
		{
			var courseGroupMessage = new CourseGroupMessageRequest()
			{
				Content = request.Content,
				CourseGroupId = (Guid)request.CourseGroupId
			};
			var res = await _messageService.SendCourseGroupMessage(courseGroupMessage, senderId);

			return new MessageResponse()
			{
				Id = res.Id,
				SenderId = senderId,
				Content = res.Content,
				Created = res.Created
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
		Task AddUserGroup(string groupName, string userName);
	}


}
