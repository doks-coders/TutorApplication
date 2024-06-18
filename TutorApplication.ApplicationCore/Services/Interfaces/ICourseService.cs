using System.Security.Claims;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface ICourseService
	{
		Task<ResponseModel> CreateCourse(CreateCourseRequest request, Guid tutorId);
		Task<ResponseModel> UpdateCourse(CreateCourseRequest request, Guid userId, Guid courseId);
		Task<ResponseModel> GetCourseInfo(Guid id, ClaimsPrincipal user);
		Task<ResponseModel> SearchCourses(PaginationRequest request);
	}
}
