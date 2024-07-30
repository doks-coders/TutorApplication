﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Responses
{
	public class UserResponse
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Guid NavigationId { get; set; }
		public string? ImageUrl { get; set; }
		public string? Interests { get; set; }
		public string Title { get; set; }
		public string About { get; set; }
		public string Email { get; set; }
	}
}
