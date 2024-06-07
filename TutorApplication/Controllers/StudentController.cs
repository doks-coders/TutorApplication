using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.SharedModels.Extensions;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.Controllers
{
	public class StudentController : BaseController
	{
		private readonly IStudentService _studentService;
		public StudentController(IStudentService studentService)
		{
			_studentService = studentService;
		}
		[Authorize]
		[HttpPost("update-profile-information")]
		public async Task<ActionResult> UpdateProfileInformation(UpdateStudentProfileInformationRequest request)
		=> await _studentService.UpdateStudentProfileInfo(request, User.GetUserId());

		[Authorize]
		[HttpGet("get-my-student-info")]
		public async Task<ActionResult> GetMyStudentInfo()
		=> await _studentService.GetStudentExtended(User.GetUserId());

		[Authorize]
		[HttpGet("join-course/{courseId:int}")]
		public async Task<ActionResult> JoinCourse(int courseId)
		=> await _studentService.JoinCourse(courseId, User.GetUserId());




	}
}
