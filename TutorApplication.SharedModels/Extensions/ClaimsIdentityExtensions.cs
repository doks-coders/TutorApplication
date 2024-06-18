using System.Security.Claims;

namespace TutorApplication.SharedModels.Extensions
{
	public static class ClaimsIdentityExtensions
	{
		public static Guid GetUserId(this ClaimsPrincipal user)
		{
			return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
		}

		public static string GetUserEmail(this ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.Name).Value;
		}
		public static string GetRole(this ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.Role).Value;
		}


	}
}
