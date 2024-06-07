using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
