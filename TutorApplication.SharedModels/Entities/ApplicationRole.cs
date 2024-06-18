using Microsoft.AspNetCore.Identity;

namespace TutorApplication.SharedModels.Entities
{
	public class ApplicationRole : IdentityRole<Guid>
	{
		public ICollection<AppUserRole> UserRoles { get; set; }
	}
}
