﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Entities
{
	public class ApplicationRole : IdentityRole<int>
	{
		public ICollection<AppUserRole> UserRoles { get; set; }
	}
}
