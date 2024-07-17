using Microsoft.AspNetCore.Mvc;
using TutorApplication.ApplicationCore.Services;
using TutorApplication.ApplicationCore.Services.Interfaces;
using static TutorApplication.ApplicationCore.Services.CourseService;

namespace TutorApplication.Controllers
{
	public class CourseController : BaseController
	{
		private readonly ICourseService _courseService;

		public CourseController(ICourseService courseService)
		{
			_courseService = courseService;
		}

		[HttpPost("update-coursename/{courseId}")]
		public async Task<ActionResult> UpdateCourseName([FromBody] CourseNameRequest request, Guid courseId)
		{
			return await _courseService.UpdateCourseName(request,courseId);
		}

		[HttpPost("update-courseabout/{courseId}")]
		public async Task<ActionResult> UpdateCourseAbout([FromBody] CourseAboutRequest request, Guid courseId)
		{
			return await _courseService.UpdateCourseAbout(request, courseId);
		}


		[HttpPost("update-courseimage/{courseId}")]
		public async Task<ActionResult> UpdateCourseImage([FromBody] CourseImageRequest request, Guid courseId)
		{
			return await _courseService.UpdateCourseImage(request, courseId);
		}

		[HttpPost("update-coursetags/{courseId}")]
		public async Task<ActionResult> UpdateCourseTags([FromBody] CourseTagsRequest request, Guid courseId)
		{
			return await _courseService.UpdateCourseTags(request, courseId);
		}

		[HttpPost("update-courseschedule/{courseId}")]
		public async Task<ActionResult> UpdateCourseSchedule([FromBody] CourseMemoRequest request, Guid courseId)
		{
			return await _courseService.UpdateCourseSchedule(request, courseId);
		}

		[HttpPost("update-coursepricing/{courseId}")]
		public async Task<ActionResult> UpdateCoursePricing([FromBody] CoursePricingRequest request, Guid courseId)
		{
			return await _courseService.UpdateCoursePricing(request, courseId,User);
		}

		[HttpGet("get-unfinished-courses")]
		public async Task<ActionResult> GetUnfinishedCourses()
		{
			return await _courseService.GetUnfinishedCourse(User);
		}
		[HttpGet("get-unfinished-course/{courseId}")]
		public async Task<ActionResult> GetUnfinishedCourse(Guid courseId)
		{
			return await _courseService.GetUnfinishedCourse(courseId);
		}

		[HttpGet("initialise-course")]
		public async Task<ActionResult> InitialiseCourse()
		{
			return await _courseService.InitialiseCourse(User);
		}
		[HttpGet("get-classes")]
		public async Task<ActionResult> GetClasses()
		{
			return await _courseService.GetAllClassesForUser(User);
		}

		[HttpGet("get-tutor-classes")]
		public async Task<ActionResult> GetAllClassesForUser()
		{
			return await _courseService.GetAllClassesForUser(User,userMode:"tutor");
		}


		






	}
}
/***
*initialise-course
update-coursename
update-courseabout
update-courseimage
update-coursetags
update-courseschedule
update-coursepricing
*/

/**
 * Get  
 * course/get-unfinished-courses
 * course/get-unfinished-course/" + id
 * 
 * POST
 * course/update-coursename/+ courseId: CourseNameRequest
 * course/update-courseabout/+ courseId: CourseAboutRequest
 * course/update-courseimage/+ courseId: CourseImageRequest
 * course/update-courseschedule/+ courseId :CourseMemoRequest
 * course/update-coursepricing/+ courseId :CoursePricingRequest
 * 
 */

