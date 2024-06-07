using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Models
{
	public class CommandPassed
	{
		public string peerId { get; set; }
		public string elementId { get; set; }
		public string command { get; set; }
	}
}
