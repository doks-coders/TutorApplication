using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface IStudentService
	{
		Task<ResponseModel> GetStudent(Guid studentId);
		Task<ResponseModel> GetStudentExtended(Guid studentId);
		Task<ResponseModel> JoinCourse(Guid courseId, Guid studentId);
		Task<ResponseModel> UpdateStudentProfileInfo(UpdateStudentProfileInformationRequest request, Guid userId);

	}
}
