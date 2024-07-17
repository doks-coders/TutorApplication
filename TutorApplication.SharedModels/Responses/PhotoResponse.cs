using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Responses
{
	public class PhotoResponse
	{
		public Guid Id { get; set; }
		public string Url { get; set; }
		public string PublicId { get; set; }
		public Guid? CourseId { get; set; }
	}
}
