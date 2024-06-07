using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Entities
{
	public class Connection
	{
		public int Id { get; set; }
		public int GroupId { get; set; }
		public Group Group { get; set; }		
		public string ConnectionURL { get; set; }
		public string GroupName { get; set; }
	}
}
