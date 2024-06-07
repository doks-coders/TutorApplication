using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.Infrastructure.Repositories.Interfaces;
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

		public async Task<MessageResponse> SendDirectMessage(MessageRequest request, int senderId, string groupName)
		{
			var directMessage = new DirectMessageRequest()
			{
				Content = request.Content,
				RecieverId = (int)request.RecieverId
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


		public async Task<MessageResponse> SendGroupMessage(MessageRequest request, int senderId, string groupName)
		{
			var courseGroupMessage = new CourseGroupMessageRequest()
			{
				Content = request.Content,
				CourseGroupId = (int)request.CourseGroupId
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

		public async Task<List<MessageResponse>> GetCourseGroupMessages(int courseGroupId, int senderId)
		{
			var responses = await _messageService.GetCourseGroupMessage(courseGroupId, senderId);

			return responses;
		}

		public async Task<List<MessageResponse>> GetDirectMessages(int recieverId, int senderId)
		{
			var responses = await _messageService.GetDirectMessages(recieverId, senderId);

			return responses;
		}




	}

	public interface IMessageHubServices
	{
		Task<MessageResponse> SendGroupMessage(MessageRequest request, int senderId, string groupName);
		Task<MessageResponse> SendDirectMessage(MessageRequest request, int senderId, string groupName);
		Task<List<MessageResponse>> GetCourseGroupMessages(int courseGroupId, int senderId);
		Task<List<MessageResponse>> GetDirectMessages(int recieverId, int senderId);
	}


}
