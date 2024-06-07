using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface ITutorService
	{
		
		Task<ResponseModel> GetTutors(PaginationRequest request);
		Task<ResponseModel> GetTutor(int tutorId);
		Task<ResponseModel> UpdateTutorProfileInfo(UpdateTutorProfileInformationRequest request, int userId);
		Task<ResponseModel> GetTutorExtended(int tutorId);
	}
}
