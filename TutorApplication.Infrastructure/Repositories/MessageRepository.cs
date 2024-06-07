using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
}
