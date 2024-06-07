using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Entities
{
	public class Group
	{
		public int Id { get; set; }	
		public string Name { get; set; }
		public List<Connection> Connections { get; set; } = new();
	}
}
