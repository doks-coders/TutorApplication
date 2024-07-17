using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorApplication.SharedModels.Entities
{
	public class ApplicationUser : IdentityUser<Guid>
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? FullName { get; set; }
		public string? FullNameBackwards { get; set; }

		public string? Title { get; set; }
		public string? Interests { get; set; }
		public string? About { get; set; }

		public DateTime DateUpdated { get; set; }
		public DateTime? LastSeen { get; set; }
		public List<Message> IncomingMessages { get; set; }
		public List<Message> OutgoingMessages { get; set; }
		public Guid NavigationId { get; set; } = Guid.NewGuid();
		public string AccountType { get; set; } = "Student";
		public string? AuthStep { get; set; }	
		public bool isProfileUpdated { get; set; } = false;
		public bool isAccountValidated { get; set; } = false;
		public bool LockAccount { get; set; } = false;
		public ICollection<AppUserRole> UserRoles { get; set; }
		public string? ImageUrl { get; set; }
		public Guid? PhotoId { get; set; }
		[ForeignKey(nameof(PhotoId))]
		[DeleteBehavior(DeleteBehavior.NoAction)]
		public Photo Photo { get; set; }
		public List<Course> Courses { get; set; }
	}

}
