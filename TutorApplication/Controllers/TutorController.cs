using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.SharedModels.Extensions;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.Controllers
{
	public class TutorController : BaseController
	{
		private readonly ITutorService _tutorService;
		private readonly ICourseService _courseService;
		public TutorController(ITutorService tutorService, ICourseService courseService)
		{
			_tutorService = tutorService;
			_courseService = courseService;
		}

		[Authorize]
		[HttpPost("create-course")]
		public async Task<ActionResult> CreateCourse([FromBody] CreateCourseRequest request)
		=> await _courseService.CreateCourse(request, User.GetUserId());

		[Authorize]
		[HttpPost("update-course/{courseId}")]
		public async Task<ActionResult> CreateCourse([FromBody] CreateCourseRequest request, Guid courseId)
		=> await _courseService.UpdateCourse(request, User.GetUserId(), courseId);


		[HttpGet("get-course-info/{courseId}")]
		public async Task<ActionResult> GetCourseInfo(Guid courseId)
		=> await _courseService.GetCourseInfo(courseId, User);

		[HttpGet("search-courses")]
		public async Task<ActionResult> SearchCourses([FromQuery] PaginationRequest request)
		=> await _courseService.SearchCourses(request);


		[HttpGet("search-tutors")]
		public async Task<ActionResult> SearchTutors([FromQuery] PaginationRequest request)
		=> await _tutorService.GetTutors(request);


		[HttpGet("get-tutor/{tutorId}")]
		public async Task<ActionResult> GetTutor(Guid tutorId)
		=> await _tutorService.GetTutor(tutorId);

		[HttpGet("get-tutor-extended/{tutorId}")]
		public async Task<ActionResult> GetTutorExtended(Guid tutorId)
		=> await _tutorService.GetTutorExtended(tutorId);

		[Authorize]
		[HttpGet("get-my-tutor-info")]
		public async Task<ActionResult> GetMyTutorInfo()
		=> await _tutorService.GetTutorExtended(User.GetUserId());


		[Authorize]
		[HttpPost("update-profile-information")]
		public async Task<ActionResult> UpdateProfileInformation([FromBody] UpdateTutorProfileInformationRequest request)
		=> await _tutorService.UpdateTutorProfileInfo(request, User.GetUserId());

	}
}
/*
 
			var options = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true,
			};
			var memos = JsonSerializer.Deserialize<IEnumerable<Memo>>(request.Memos,options);
			var weeks = memos.ConvertMemosToWeekChapters();
			var weekLength = weeks.Count();


 
 */