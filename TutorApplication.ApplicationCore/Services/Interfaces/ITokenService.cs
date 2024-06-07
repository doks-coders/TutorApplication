using TutorApplication.SharedModels.Entities;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(ApplicationUser user);
	}
}
