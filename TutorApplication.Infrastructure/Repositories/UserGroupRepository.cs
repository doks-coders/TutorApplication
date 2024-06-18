using TutorApplication.Infrastructure.Data;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;

namespace TutorApplication.Infrastructure.Repositories
{
	public class UserGroupRepository : BaseRepository<UserGroup>, IUserGroupRepository
	{
		public UserGroupRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
	public interface IUserGroupRepository : IBaseRepository<UserGroup>
	{

	}
}
