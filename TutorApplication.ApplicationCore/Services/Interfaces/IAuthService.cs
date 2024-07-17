using System.Security.Claims;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using static TutorApplication.ApplicationCore.Services.AuthService;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface IAuthService
	{
		Task<ResponseModel> Register(RegisterUserRequest request);
		Task<ResponseModel> Login(LoginUserRequest request);
		Task<ResponseModel> UserExist(ClaimsPrincipal user);
		Task<ResponseModel> CheckIfProfileUpdated(ClaimsPrincipal user);
		Task<ResponseModel> GetUserInformation(ClaimsPrincipal user);
		Task<ResponseModel> UpdateAbout(ClaimsPrincipal user, AboutRequest request);
		Task<ResponseModel> UpdateImage(ClaimsPrincipal user, ImageRequest request);
		Task<ResponseModel> UpdateFullName(ClaimsPrincipal user, FullNameRequest request);
		Task<ResponseModel> UpdateInterests(ClaimsPrincipal user, InterestsRequest request);
		Task<ResponseModel> UpdateTitle(ClaimsPrincipal user, TitleRequest request);
		Task<ResponseModel> GetUpdatedState(ClaimsPrincipal user);
	}
}
