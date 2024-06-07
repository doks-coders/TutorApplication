using TutorApplication.Infrastructure.Data;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;

namespace TutorApplication.Infrastructure.Repositories
{
	public class GroupRepository : BaseRepository<Group>, IGroupRepository
	{
		public GroupRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
