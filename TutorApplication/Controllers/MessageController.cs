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

		[HttpGet("get-student-contacts/{studentId}")]
		public async Task<ActionResult> GetContactsForStudents(Guid studentId)
		=> await _messageService.GetContactsForStudents(studentId);

		[HttpGet("get-tutor-contacts/{tutorId}")]
		public async Task<ActionResult> GetContactsForTutors(Guid tutorId)
		=> await _messageService.GetContactsForTutors(tutorId);

		[HttpPost("delete-message")]
		public async Task<ActionResult> DeleteMessage()
		{
			return Ok();
		}
	}
}
