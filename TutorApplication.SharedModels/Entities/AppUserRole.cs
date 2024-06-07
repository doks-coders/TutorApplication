using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Entities
{
	public class AppUserRole : IdentityUserRole<int>
	{
		public ApplicationUser AppUser { get; set; }
		public ApplicationRole AppRole { get; set; }	
	}
}
