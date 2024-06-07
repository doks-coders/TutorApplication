using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface IStudentService
	{
		Task<ResponseModel> GetStudent(int studentId);
		Task<ResponseModel> GetStudentExtended(int studentId);
		Task<ResponseModel> JoinCourse(int courseId, int studentId);
		Task<ResponseModel> UpdateStudentProfileInfo(UpdateStudentProfileInformationRequest request, int userId);

	}
}
