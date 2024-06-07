using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Requests
{
	public class DirectMessageRequest
	{
		public int RecieverId { get; set; }
		public string Content { get; set; }
	}
}
