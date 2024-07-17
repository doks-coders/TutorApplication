namespace TutorApplication.Infrastructure.Repositories.Interfaces
{
	public interface IUnitOfWork
	{
		IApplicationUserRepository Users { get; }
		ICourseRepository Courses { get; }
		public ICourseStudentRepository CourseStudents { get; }
		public IMessageRepository Messages { get; }
		public IConnectionRepository Connections { get; }
		public IGroupRepository Groups { get; }
		public IUserGroupRepository UserGroups { get; }
		public IPhotoRepository Photos { get; }
		IMissedMessageRespository MissedMessage { get; }
		Task<bool> SaveChanges();
	}
}
