using Microsoft.AspNetCore.Identity;

namespace TutorApplication.SharedModels.Entities
{
	public class ApplicationRole : IdentityRole<int>
	{
		public ICollection<AppUserRole> UserRoles { get; set; }
	}
}
