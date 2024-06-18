using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses.Messages;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface IMessageService
	{
		Task<List<MessageResponse>> GetDirectMessages(Guid recieverId, Guid senderId);

		Task<List<MessageResponse>> GetCourseGroupMessage(Guid courseGroupId, Guid senderId);

		Task<Message> SendDirectMessage(DirectMessageRequest request, Guid senderId);

		Task<Message> SendCourseGroupMessage(CourseGroupMessageRequest request, Guid senderId);

		Task<ResponseModel> DeleteMessage(Guid messageId);

		Task<ResponseModel> GetContactsForStudents(Guid studentId);

		Task<ResponseModel> GetContactsForTutors(Guid tutorId);

		Task<List<DisplayMessageContact>> GetContactsForStudentsWithHub(Guid studentId);

		Task<List<DisplayMessageContact>> GetContactsForTutorswithHub(Guid tutorId);

	}
}

/*

Get sender is the User

reciever is the person's id passed
 * */