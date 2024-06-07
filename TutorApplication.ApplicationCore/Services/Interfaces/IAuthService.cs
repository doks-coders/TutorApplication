using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface IAuthService
	{
		Task<ResponseModel> Register(RegisterUserRequest request);
		Task<ResponseModel> Login(LoginUserRequest request);
	}
}
