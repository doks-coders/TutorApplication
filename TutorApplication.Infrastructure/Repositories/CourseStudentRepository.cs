using Microsoft.EntityFrameworkCore;
using TutorApplication.Infrastructure.Data;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;

namespace TutorApplication.Infrastructure.Repositories
{
	public class CourseStudentRepository : BaseRepository<CourseStudent>, ICourseStudentRepository
	{
		private readonly ApplicationDbContext _context;
		public CourseStudentRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<IEnumerable<CourseStudent>> GetOneCourseStudentForStudent(Guid studentId)
		{
			return await _context.CourseStudents.Where(u => u.StudentId == studentId)
				.Include(u => u.Course)
				.ThenInclude(u => u.Students)
				.Include(u => u.Course)
				.ThenInclude(u => u.Tutor)
				.ToListAsync();
		}
	}
}
