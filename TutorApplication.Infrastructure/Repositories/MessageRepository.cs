using TutorApplication.Infrastructure.Data;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;

namespace TutorApplication.Infrastructure.Repositories
{
	public class MessageRepository : BaseRepository<Message>, IMessageRepository
	{
		public MessageRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
	public interface IMissedMessageRespository : IBaseRepository<MissedMessage>
	{

	}

	public class MissedMessageRespository : BaseRepository<MissedMessage>, IMissedMessageRespository
	{
		public MissedMessageRespository(ApplicationDbContext context) : base(context)
		{
		}
	}



}
