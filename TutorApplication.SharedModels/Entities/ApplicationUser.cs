﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Entities
{
	public class ApplicationUser:IdentityUser<int>
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? FullName { get; set; }
		public string? FullNameBackwards { get; set; }

		public string? Title { get; set; }	
		public string? About { get; set; }

		public DateTime DateUpdated { get; set; }	
		public List<Message> IncomingMessages { get; set; }
		public List<Message> OutgoingMessages { get; set; }
		public Guid NavigationId { get; set; } = Guid.NewGuid();
		public string AccountType { get; set; } = "Student";
		public bool isProfileUpdated { get; set; } = false;
		public bool isAccountValidated { get; set; } = false;
		public bool LockAccount { get; set; } = false;
		public ICollection <AppUserRole> UserRoles { get; set; }

		public List<Course> Courses { get; set; }
	}
}