using TutorApplication.Infrastructure.Data;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;

namespace TutorApplication.Infrastructure.Repositories
{
	public class ConnectionRepository : BaseRepository<Connection>, IConnectionRepository
	{
		public ConnectionRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
