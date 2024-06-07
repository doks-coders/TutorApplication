using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Requests
{
	public class CourseGroupMessageRequest
	{
		public int CourseGroupId { get; set; }
		public string Content { get; set; }
	}
}
