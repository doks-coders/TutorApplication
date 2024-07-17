using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using static TutorApplication.ApplicationCore.Services.CourseService;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface ICourseService
	{
		Task<ResponseModel> CreateCourse(CreateCourseRequest request, Guid tutorId);
		Task<ResponseModel> UpdateCourse(CreateCourseRequest request, Guid userId, Guid courseId);
		Task<ResponseModel> GetCourseInfo(Guid id, ClaimsPrincipal user);
		Task<ResponseModel> SearchCourses(PaginationRequest request);
		Task<ResponseModel> GetAllClassesForUser(ClaimsPrincipal user, string userMode = "student");

		Task<ResponseModel> GetUnfinishedCourse(ClaimsPrincipal user);
		Task<ResponseModel> GetUnfinishedCourse(Guid courseId);
		Task<ResponseModel> InitialiseCourse(ClaimsPrincipal user);
		Task<ResponseModel> UpdateCourseName(CourseNameRequest request, Guid courseid);
		Task<ResponseModel> UpdateCourseAbout(CourseAboutRequest request, Guid courseid);
		Task<ResponseModel> UpdateCourseImage(CourseImageRequest request, Guid courseId);
		Task<ResponseModel> UpdateCourseTags(CourseTagsRequest request, Guid courseId);
		Task<ResponseModel> UpdateCourseSchedule(CourseMemoRequest request, Guid courseid);
		Task<ResponseModel> UpdateCoursePricing(CoursePricingRequest request, Guid courseId, ClaimsPrincipal user);



	}
}
