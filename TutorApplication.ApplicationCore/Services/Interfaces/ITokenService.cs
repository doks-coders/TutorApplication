using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApplication.SharedModels.Entities;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(ApplicationUser user);
	}
}
