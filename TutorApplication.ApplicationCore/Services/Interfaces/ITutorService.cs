using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface ITutorService
	{

		Task<ResponseModel> GetTutors(PaginationRequest request);
		Task<ResponseModel> GetTutor(Guid tutorId);
		Task<ResponseModel> UpdateTutorProfileInfo(UpdateTutorProfileInformationRequest request, Guid userId);
		Task<ResponseModel> GetTutorExtended(Guid tutorId);
	}
}
