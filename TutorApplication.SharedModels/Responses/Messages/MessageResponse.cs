using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Responses.Messages
{
	public class MessageResponse
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public DateTime Created { get; set; }
		public int SenderId { get; set; }	
	}
}
