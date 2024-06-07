using System.Security.Claims;

namespace TutorApplication.SharedModels.Extensions
{
	public static class ClaimsIdentityExtensions
	{
		public static int GetUserId(this ClaimsPrincipal user)
		{
			return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
		}

		public static string GetUserEmail(this ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.Name).Value;
		}
	}
}
