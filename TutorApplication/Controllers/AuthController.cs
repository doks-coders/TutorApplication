using Microsoft.AspNetCore.Mvc;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.SharedModels.Requests;
using static TutorApplication.ApplicationCore.Services.AuthService;

namespace TutorApplication.Controllers
{
	public class AuthController : BaseController
	{
		private readonly IAuthService _authService;
		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}
		[HttpPost("register")]
		public async Task<ActionResult> Register([FromBody] RegisterUserRequest registerRequest)
		=> await _authService.Register(registerRequest);

		[HttpPost("login")]
		public async Task<ActionResult> Login([FromBody] LoginUserRequest loginUserRequest)
		=> await _authService.Login(loginUserRequest);


		[HttpGet("user-exists")]
		public async Task<ActionResult> CheckUserExistence()
		=> await _authService.UserExist(User);


		[HttpPost("account-setup/update-fullname")]
		public async Task<ActionResult> UpdateFullName([FromBody] FullNameRequest request)
		=> await _authService.UpdateFullName(User, request);

		[HttpPost("account-setup/update-title")]
		public async Task<ActionResult> UpdateTitle([FromBody] TitleRequest request)
		=> await _authService.UpdateTitle(User, request);

		[HttpPost("account-setup/update-interests")]
		public async Task<ActionResult> UpdateInterests([FromBody] InterestsRequest request)
		=> await _authService.UpdateInterests(User, request);

		[HttpPost("account-setup/update-about")]
		public async Task<ActionResult> UpdateAbout([FromBody] AboutRequest request)
		=> await _authService.UpdateAbout(User, request);

		[HttpPost("account-setup/update-image")]
		public async Task<ActionResult> UpdateImage([FromBody] ImageRequest request)
		=> await _authService.UpdateImage(User, request);


		[HttpGet("get-user-info")]
		public async Task<ActionResult> GetUserInformation()
		=> await _authService.GetUserInformation(User);

		[HttpGet("profile-updated-state")]
		public async Task<ActionResult> GetUpdatedState()
		=> await _authService.GetUpdatedState(User);


		/***
  * account-setup/update-fullname
  * account-setup/update-title
  * account-setup/update-interests
  * account-setup/update-about
  * account-setup/update-image
  * get-user-info
  * profile-updated-state
  */
	}
}
