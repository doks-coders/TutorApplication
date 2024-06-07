using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Requests
{
	public class UpdateTutorProfileInformationRequest
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Title { get; set; }
		public string About { get; set; }
	}
}
	
