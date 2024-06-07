using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Models
{
	public class VideoState
	{
		public string id { get; set; }
		public bool showVideo { get; set; }
		public bool playAudio { get; set; }
		public string peerId { get; set; }
	}
}
