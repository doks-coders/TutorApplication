using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Entities
{
	public class MissedMessage:BaseEntity
	{
		public string UserName { get; set; } = "";
		public Guid SenderId { get; set; }
		public Guid RecieverId { get; set; }
		public Guid CourseGroupId { get; set; }
		public string? isGroup { get; set; }
	}
}
