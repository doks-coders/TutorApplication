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
