using TutorApplication.SharedModels.Entities;

namespace TutorApplication.Infrastructure.Repositories.Interfaces
{
	public interface ICourseRepository : IBaseRepository<Course>
	{
		Task<IEnumerable<Course>> GetOneTutorCourse(Guid tutorId);

	}
	
}
