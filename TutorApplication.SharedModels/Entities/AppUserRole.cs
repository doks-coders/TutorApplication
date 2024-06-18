using Microsoft.AspNetCore.Identity;

namespace TutorApplication.SharedModels.Entities
{
	public class AppUserRole : IdentityUserRole<Guid>
	{
		public ApplicationUser AppUser { get; set; }
		public ApplicationRole AppRole { get; set; }
	}
}
