using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Entities
{
	public class Photo:BaseEntity
	{
		public string Url { get; set; }
		public string PublicId { get; set; }
		public Course Course { get; set; }		
		public Guid? CourseId { get; set; }
		public Guid? MessageId { get; set; }
		public Message Message { get; set; }
	}
}
