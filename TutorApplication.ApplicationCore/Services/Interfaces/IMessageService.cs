using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses.Messages;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface IMessageService
	{
		Task<List<MessageResponse>> GetDirectMessages(int recieverId, int senderId);

		Task<List<MessageResponse>> GetCourseGroupMessage(int courseGroupId, int senderId);

		Task<Message> SendDirectMessage(DirectMessageRequest request, int senderId);

		Task<Message> SendCourseGroupMessage(CourseGroupMessageRequest request, int senderId);

		Task<ResponseModel> DeleteMessage(int messageId);

		Task<ResponseModel> GetContactsForStudents(int studentId);
		Task<ResponseModel> GetContactsForTutors(int tutorId);

	}
}

/*

Get sender is the User

reciever is the person's id passed
 * */