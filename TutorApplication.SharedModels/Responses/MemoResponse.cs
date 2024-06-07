using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Responses
{
	public class MemoResponse
	{
		public string Type { get; set; } = "";
		public string BookInfo { get; set; } = "";
		public string Time { get; set; } = "";
		public DateTime Date { get; set; }
	}
}
