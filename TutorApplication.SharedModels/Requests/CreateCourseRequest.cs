using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Requests
{
	public class CreateCourseRequest
	{
		public string CourseTitle { get; set; }
		public string About { get; set; }
		public int Price { get; set; }
		public string Currency { get; set; }
		public string Memos { get; set; }
	}
}
