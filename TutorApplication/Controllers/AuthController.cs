using Microsoft.AspNetCore.Mvc;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.SharedModels.Requests;

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
	}
}
