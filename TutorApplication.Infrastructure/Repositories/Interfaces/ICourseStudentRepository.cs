using System.Linq.Expressions;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses;

namespace TutorApplication.Infrastructure.Repositories.Interfaces
{
	public interface ICourseStudentRepository : IBaseRepository<CourseStudent>
	{
		Task<IEnumerable<CourseStudent>> GetOneCourseStudentForStudent(Guid studentId);
		Task<IEnumerable<CourseStudent>> GetOneCourseStudentForTutor(Guid tutorId);

		Task<PaginationResponse> GetStudentsPaginationItems(PaginationRequest request, Expression<Func<CourseStudent, bool>> query, string? includeProperties = null);
		Task<PaginationResponse> GetTutorsPaginationItems(PaginationRequest request, Expression<Func<CourseStudent, bool>> query, string? includeProperties = null);
		Task<PaginationResponse> GetCoursePaginationItems(PaginationRequest request, Expression<Func<CourseStudent, bool>> query, string? includeProperties = null);

	}
}
