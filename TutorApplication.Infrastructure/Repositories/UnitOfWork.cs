using TutorApplication.Infrastructure.Data;
using TutorApplication.Infrastructure.Repositories.Interfaces;

namespace TutorApplication.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		public IApplicationUserRepository Users { get; }
		public ICourseRepository Courses { get; }
		public ICourseStudentRepository CourseStudents { get; }
		public IMessageRepository Messages { get; }
		public IConnectionRepository Connections { get; }
		public IGroupRepository Groups { get; }
		private readonly ApplicationDbContext _context;

		public UnitOfWork(ApplicationDbContext context)
		{

			_context = context;
			Users = new ApplicationUserRepository(context);
			CourseStudents = new CourseStudentRepository(context);
			Courses = new CourseRepository(context);
			Messages = new MessageRepository(context);
			Connections = new ConnectionRepository(context);
			Groups = new GroupRepository(context);
		}

		public async Task<bool> SaveChanges()
		{
			return 0 < await _context.SaveChangesAsync();
		}
	}
}
