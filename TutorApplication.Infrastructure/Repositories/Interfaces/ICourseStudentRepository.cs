using TutorApplication.SharedModels.Entities;

namespace TutorApplication.Infrastructure.Repositories.Interfaces
{
	public interface ICourseStudentRepository : IBaseRepository<CourseStudent>
	{
		Task<IEnumerable<CourseStudent>> GetOneCourseStudentForStudent(Guid studentId);
	}
}
