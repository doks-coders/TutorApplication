using Azure.Core;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TutorApplication.ApplicationCore.Extensions;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Enums;
using TutorApplication.SharedModels.Extensions;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses;
using TutorApplication.SharedModels.StaticDefinitions;
using TutorApplication.SharedModels.Validations.Auth;

namespace TutorApplication.ApplicationCore.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ITokenService _tokenService;
		private readonly IUnitOfWork _unitOfWork;

		public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService, IUnitOfWork unitOfWork)
		{
			_userManager = userManager;
			_tokenService = tokenService;

			_unitOfWork = unitOfWork;
		}

		public async Task<ResponseModel> UserExist(ClaimsPrincipal user)
		{
			string? email = null;
			try
			{
				email = user.GetUserEmail();
			}
			catch (Exception ex) { }
			if (!_userManager.Users.Any(u => u.Email == email)) return ResponseModel.Send("User Does Not Exist");
			return ResponseModel.Send("User Exists");
		}

		public class FullNameRequest
		{
			public string FirstName { get; set; }
			public string LastName { get; set; }
		}

		public async Task<ResponseModel> CheckIfProfileUpdated(ClaimsPrincipal user)
		{
			var retrievedUser = await _unitOfWork.Users.GetItem(u => u.Id == user.GetUserId());
			return ResponseModel.Send(retrievedUser.isProfileUpdated);
		}

	
		public async Task<ResponseModel> GetUserInformation(ClaimsPrincipal user)
		{
			var retrievedUser = await _unitOfWork.Users.GetItem(u => u.Id == user.GetUserId(), includeProperties: "Photo") ;
			var response = new UnfinishedUserResponse()
			{
				About = retrievedUser.About,
				FirstName = retrievedUser.FirstName,
				LastName = retrievedUser.LastName,
				ImageUrl = retrievedUser.ImageUrl,
				Photo = retrievedUser.Photo !=null? retrievedUser.Photo.ConvertPhotoToPhotoResponse():null,
				Interests = retrievedUser.Interests,
				Title = retrievedUser.Title,
				AuthStep = retrievedUser.AuthStep
			};
			return ResponseModel.Send(retrievedUser);
		}


		public async Task<ResponseModel> UpdateFullName(ClaimsPrincipal user,FullNameRequest request)
		{
			var id = user.GetUserId();
			var retrievedUser = await _unitOfWork.Users.GetItem(u => u.Id == id);
			retrievedUser.FirstName = request.FirstName;
			retrievedUser.LastName = request.LastName;
			retrievedUser.AuthStep = ProfileSetup.ProfileImage;
			await _unitOfWork.SaveChanges();
			return ResponseModel.Send(ProfileSetup.ProfileImage);
		}

		public class InterestsRequest
		{
			public string Interests { get; set; }
		}
		
		public async Task<ResponseModel> UpdateInterests(ClaimsPrincipal user, InterestsRequest request)
		{
			var id = user.GetUserId();
			var retrievedUser = await _unitOfWork.Users.GetItem(u => u.Id == id);
			retrievedUser.Interests = request.Interests;
			retrievedUser.AuthStep = ProfileSetup.About;
			await _unitOfWork.SaveChanges();
			return ResponseModel.Send(ProfileSetup.About);
		}


		public class TitleRequest
		{
			public string Title { get; set; }
		}
	
		public async Task<ResponseModel> UpdateTitle(ClaimsPrincipal user, TitleRequest request)
		{
			var id = user.GetUserId();
			var retrievedUser = await _unitOfWork.Users.GetItem(u => u.Id == id);
			retrievedUser.Title = request.Title;
			retrievedUser.AuthStep = ProfileSetup.Interests;

			await _unitOfWork.SaveChanges();
			return ResponseModel.Send(ProfileSetup.Interests);
		}

		public class ImageRequest
		{
			public string ImageUrl { get; set; }
		}
		public async Task<ResponseModel> UpdateImage(ClaimsPrincipal user, ImageRequest request)
		{
			var id = user.GetUserId();
			var retrievedUser = await _unitOfWork.Users.GetItem(u => u.Id == id);
			retrievedUser.ImageUrl = request.ImageUrl;
			retrievedUser.AuthStep = ProfileSetup.Occupation;

			await _unitOfWork.SaveChanges();
			return ResponseModel.Send(ProfileSetup.Occupation);
		}

		public class AboutRequest
		{
			public string About { get; set; }
		}
		public async Task<ResponseModel> UpdateAbout(ClaimsPrincipal user, AboutRequest request)
		{
			var id = user.GetUserId();
			var retrievedUser = await _unitOfWork.Users.GetItem(u => u.Id == id);
			retrievedUser.About = request.About;
			retrievedUser.AuthStep = ProfileSetup.Completed;
			retrievedUser.isProfileUpdated = true;

			await _unitOfWork.SaveChanges();
			return ResponseModel.Send();
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

			if (_userManager.Users.Any(u => u.Email == request.Email)) throw new CustomException(ErrorCodes.UserExist);
			ApplicationUser user = new();
			user.Email = request.Email;
			user.UserName = request.Email;
			user.AccountType = request.AccountType;
			var response = await _userManager.CreateAsync(user, request.Password);

			if (!response.Succeeded) throw new CustomException(response.Errors);
			var apiResponse = new AuthUserResponse() { Token = _tokenService.CreateToken(user), UserName = user.Email };
			return ResponseModel.Send(apiResponse);
		}
	}
}
