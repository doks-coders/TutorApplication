using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Requests
{
	public class MessageRequest
	{
		public bool isGroup { get; set; } = false;

		public int? RecieverId { get; set; }
		
		public int? CourseGroupId { get; set; }
		public string Content { get; set; }
	}
}
