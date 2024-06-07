using Microsoft.AspNetCore.Mvc;
using TutorApplication.ApplicationCore.Services.Interfaces;

namespace TutorApplication.Controllers
{
	public class MessageController : BaseController
	{
		private readonly IMessageService _messageService;
		public MessageController(IMessageService messageService) 
		{
			_messageService = messageService;
		}
		/*
		[HttpGet("get-user-messages")]
		public async Task<ActionResult> GetUserMessages(int recieverId)
		=> await _messageService.GetDirectMessages(recieverId, 2);


		[HttpGet("get-course-group-messages")]
		public async Task<ActionResult> GetCourseGroupMessage(int courseeGroupId)
		=> await _messageService.GetCourseGroupMessage(courseeGroupId, 2);



		[HttpPost("send-direct-message")]
		public async Task<ActionResult> SendMessage(DirectMessageRequest request)
		=> await _messageService.SendDirectMessage(request, 2);

		[HttpPost("send-course-group-message")]
		public async Task<ActionResult> SendCourseGroup(CourseGroupMessageRequest request)
		=> await _messageService.SendCourseGroupMessage(request, 2);
		*/

		[HttpGet("get-student-contacts/{studentId:int}")]
		public async Task<ActionResult> GetContactsForStudents(int studentId)
		=> await _messageService.GetContactsForStudents(studentId);

		[HttpGet("get-tutor-contacts/{tutorId:int}")]
		public async Task<ActionResult> GetContactsForTutors(int tutorId)
		=> await _messageService.GetContactsForTutors(tutorId);

		[HttpPost("delete-message")]
		public async Task<ActionResult> DeleteMessage()
		{
			return Ok();
		}
	}
}
