using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Entities
{
	public class BaseEntity
	{
		public int Id { get; set; }
		public Guid NavigationId { get; set; } = Guid.NewGuid();
		public DateTime Created { get; set; } = DateTime.UtcNow;
		public DateTime Updated { get; set; } = DateTime.UtcNow;

		public BaseEntity()
		{
			Updated = DateTime.UtcNow;
		}
	}
}
