using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Enums;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses;
using TutorApplication.SharedModels.Validations.Auth;

namespace TutorApplication.ApplicationCore.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ITokenService _tokenService;

		public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
		{
			_userManager = userManager;
			_tokenService = tokenService;
		}

		
		public async Task<ResponseModel> Login(LoginUserRequest request)
		{

			var validator = new LoginUserValidators();
			var res = await validator.ValidateAsync(request);
			if (!res.IsValid) throw new CustomException(res.Errors);
			
			if (!_userManager.Users.Any(u => u.Email == request.Email)) throw new CustomException(ErrorCodes.UserDoesNotExist);
			var user = await _userManager.FindByEmailAsync(request.Email);
			
			var response = await _userManager.CheckPasswordAsync(user, request.Password);
			if (!response) throw new CustomException(ErrorCodes.IncorrectPassword);

			var apiResponse = new AuthUserResponse() { Token = _tokenService.CreateToken(user), UserName = user.Email };
			return ResponseModel.Send(apiResponse);
		}

		public async Task<ResponseModel> Register(RegisterUserRequest request)
		{
			var validator = new RegisterUserValidators();
			var res = await validator.ValidateAsync(request);
			if (!res.IsValid) throw new CustomException(res.Errors);

			if (_userManager.Users.Any(u => u.Email == request.Email)) throw  new CustomException(ErrorCodes.UserExist);
			ApplicationUser user = new();
			user.Email = request.Email;
			user.UserName = request.Email;
			user.AccountType = request.AccountType;
			var response =  await _userManager.CreateAsync(user, request.Password);

			if (!response.Succeeded) throw new CustomException(response.Errors);
			var apiResponse = new AuthUserResponse() { Token = _tokenService.CreateToken(user), UserName = user.Email };
			return ResponseModel.Send(apiResponse);
		}
	}
}
