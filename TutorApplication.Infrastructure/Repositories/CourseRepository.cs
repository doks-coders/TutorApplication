using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TutorApplication.Infrastructure.Data;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;

namespace TutorApplication.Infrastructure.Repositories
{
	public class CourseRepository : BaseRepository<Course>, ICourseRepository
	{
		private readonly ApplicationDbContext _context;
		public CourseRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		

		public async Task<IEnumerable<Course>> GetOneTutorCourse(Guid tutorId)
		{
			return await _context.Courses.Where(u => u.TutorId == tutorId && u.isDetailsCompleted==true ).Include(u => u.Tutor)
				.Include(u=>u.Students).ToListAsync();
		}
	}
}
