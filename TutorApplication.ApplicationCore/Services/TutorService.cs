using System.Linq.Expressions;
using System.Text.Json;
using TutorApplication.ApplicationCore.Extensions;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.ApplicationCore.Utils;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Constants;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Enums;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses;

namespace TutorApplication.ApplicationCore.Services
{
	public class TutorService : ITutorService
	{
		private readonly IUnitOfWork _unitOfWork;
		public TutorService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}



		private Expression<Func<ApplicationUser, bool>> Search(PaginationRequest request)
		{
			return u =>
			   (request.TutorName == "All" || u.FullName.ToLower().Contains(request.TutorName.ToLower()) || u.FullNameBackwards.ToLower().Contains(request.TutorName.ToLower()))
			   &&
			   u.AccountType == RoleConstants.Tutor
				&& u.isProfileUpdated == true;



			//(u.CourseTitle.ToLower().Contains(request.CourseKeyWord.ToLower()) || pagination.CourseKeyWord == "All") && u.isDetailsCompleted == true;


		}

		public async Task<ResponseModel> GetTutors(PaginationRequest request)
		{
			var items = await _unitOfWork.Users.GetPaginationItems(request, Search(request));
			items.Items = ((IEnumerable<ApplicationUser>)items.Items).Select(e => new TutorResponse()
			{
				Id = e.Id,
				FirstName = e.FirstName,
				LastName = e.LastName,
				About = e.About,
				Title = e.Title,
				Email = e.Email,
				ImageUrl = e.ImageUrl
			}).ToList();

			return ResponseModel.Send(items);
		}

		public async Task<ResponseModel> GetTutor(Guid tutorId)
		{
			var item = await _unitOfWork.Users.GetItem(u => u.Id == tutorId);
			if (item == null) throw new CustomException(ErrorCodes.UserDoesNotExist);
			var response = new TutorResponse()
			{
				FirstName = item.FirstName,
				LastName = item.LastName,
				About = item.About,
				Title = item.Title,
				Email = item.Email,
				ImageUrl = item.ImageUrl,
			};

			return ResponseModel.Send(response);
		}

		public async Task<ResponseModel> GetTutorExtended(Guid tutorId)
		{
			var user = await _unitOfWork.Users.GetItem(u => u.Id == tutorId);
			if(user.AccountType != "Student")
			{
				var tutorResponse = await GetTutorDetails(tutorId);
				return ResponseModel.Send(tutorResponse);

			}
			var studentResponse = await GetStudentExtended(tutorId);

			return ResponseModel.Send(studentResponse);
		}


		public async Task<StudentExtendedResponse> GetStudentExtended(Guid studentId)
		{
			JsonSerializerOptions options = new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			var user = await _unitOfWork.Users.GetItem(u => u.Id == studentId);
			if (user == null) throw new CustomException(ErrorCodes.UserDoesNotExist);


			var response = new StudentExtendedResponse()
			{
				About = user.About,
				NavigationId = user.NavigationId,
				Email = user.Email,
				Interests = user.Interests,
				ImageUrl = user.ImageUrl,
				FirstName = user.FirstName,
				LastName = user.LastName,
				AccountType = user.AccountType,
				Id = user.Id,
				Title = user.Title,
			};
			var item = await _unitOfWork.CourseStudents.GetOneCourseStudentForStudent(studentId);

			response.Courses = item.ConvertCourseToCourseResponse();


			var tutors = item.Select(e => new TutorResponse()
			{
				Id = e.Tutor.Id,
				NavigationId = e.Tutor.NavigationId,
				About = e.Tutor.About,
				Title = e.Tutor.Title,
				ImageUrl = e.Tutor.ImageUrl,
				Email = e.Tutor.Email,
				FirstName = e.Tutor.FirstName,
				LastName = e.Tutor.LastName,
			}).GroupBy(e => e.Id).Select(g => g.First()).ToList();



			response.Tutors = tutors.ToList();


			return response;

		}


		private async Task<TutorExtendedResponse> GetTutorDetails(Guid tutorId)
		{

			var user = await _unitOfWork.Users.GetItem(u => u.Id == tutorId);
			var courses = await _unitOfWork.Courses.GetOneTutorCourse(tutorId);

			//Tutor
			JsonSerializerOptions options = new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			var courseStudents = await _unitOfWork.CourseStudents.GetItems(u => u.TutorId == tutorId && u.StudentId != tutorId, includeProperties: "Student");
			var response = new TutorExtendedResponse()
			{
				Id = user.Id,
				NavigationId = user.NavigationId,
				FirstName = user.FirstName,
				LastName = user.LastName,
				About = user.About,
				Title = user.Title,
				Email = user.Email,
				Interests = user.Interests,
				ImageUrl = user.ImageUrl,
				AccountType = user.AccountType,
				Courses = courses.ConvertCourseToCourseResponse(),
				Students = courseStudents.Select(u => new StudentResponse()
				{
					Id = u.StudentId,
					NavigationId = u.Student.NavigationId,
					ImageUrl = u.Student.ImageUrl,
					Title = u.Student.Title,
					Email = u.Student.Email,
					FirstName = u.Student.FirstName,
					LastName = u.Student.LastName,
			
				}).GroupBy(u => u.Id).Select(u => u.First()).ToList()
			};

			return response;
				 
		}


		public async Task<ResponseModel> UpdateTutorProfileInfo(UpdateTutorProfileInformationRequest request, Guid userId)
		{
			var user = await _unitOfWork.Users.GetItem(u => u.Id == userId);
			if (user == null) throw new CustomException(ErrorCodes.UserDoesNotExist);

			user.FirstName = request.FirstName;
			user.LastName = request.LastName;
			user.FullName = request.LastName + " " + request.FirstName;
			user.FullNameBackwards = request.FirstName + " " + request.LastName;
			user.About = request.About;
			user.Title = request.Title;
			user.DateUpdated = DateTime.UtcNow;
			user.isProfileUpdated = true;
			if (await _unitOfWork.SaveChanges())
			{
				var response = new TutorResponse()
				{
					Id = user.Id,
					FirstName = user.FirstName,
					LastName = user.LastName,
					About = user.About,
					Title = user.Title,
					Email = user.Email
				};
				return ResponseModel.Send(response);
			}
			throw new CustomException(ErrorCodes.ErrorWhileSaving);

		}

	}
}



