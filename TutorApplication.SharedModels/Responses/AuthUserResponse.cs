using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Responses
{
	public class AuthUserResponse
	{
		public string UserName { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;
	}
}
