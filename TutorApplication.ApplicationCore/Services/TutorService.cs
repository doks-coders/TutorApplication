using System.Linq.Expressions;
using System.Text.Json;
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
			   (string.IsNullOrEmpty(request.TutorName) || request.TutorName == "All" || u.FullName.ToLower().Contains(request.TutorName.ToLower()) || u.FullNameBackwards.ToLower().Contains(request.TutorName.ToLower()))
			   &&
			   (u.AccountType == RoleConstants.Tutor)
				&&
			   (u.isProfileUpdated == true);

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
				Email = e.Email
			}).ToList();

			return ResponseModel.Send(items);
		}

		public async Task<ResponseModel> GetTutor(int tutorId)
		{
			var item = await _unitOfWork.Users.GetItem(u => u.Id == tutorId);
			if (item == null) throw new CustomException(ErrorCodes.UserDoesNotExist);
			var response = new TutorResponse()
			{
				FirstName = item.FirstName,
				LastName = item.LastName,
				About = item.About,
				Title = item.Title,
				Email = item.Email
			};

			return ResponseModel.Send(response);
		}

		public async Task<ResponseModel> GetTutorExtended(int tutorId)
		{
			var user = await _unitOfWork.Users.GetItem(u => u.Id == tutorId, includeProperties: "Courses");

			if (user == null) throw new CustomException(ErrorCodes.UserDoesNotExist);
			JsonSerializerOptions options = new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			var courseStudents = await _unitOfWork.CourseStudents.GetItems(u => u.TutorId == tutorId, includeProperties: "Student");

			var response = new TutorExtendedResponse()
			{
				NavigationId = user.NavigationId,
				FirstName = user.FirstName,
				LastName = user.LastName,
				About = user.About,
				Title = user.Title,
				Email = user.Email,
				Courses = user.Courses.Select(e => new CourseResponse()
				{
					Id = e.Id,
					NavigationId = e.NavigationId,
					About = e.About,
					CourseTitle = e.CourseTitle,
					Currency = e.Currency,
					Price = e.Price,
					Weeks = JsonSerializer.Deserialize<IEnumerable<Memo>>(e.Memos, options)
					.ConvertMemosToWeekChapters().Count()
				}).ToList(),

				Students = courseStudents.Select(u => new StudentResponse()
				{
					Id = u.StudentId,
					NavigationId = u.Student.NavigationId,
					Title = u.Student.Title,
					Email = u.Student.Email,
					FirstName = u.Student.FirstName,
					LastName = u.Student.LastName
				}).GroupBy(u => u.Id).Select(u => u.First()).ToList()
			};



			return ResponseModel.Send(response);
		}


		public async Task<ResponseModel> UpdateTutorProfileInfo(UpdateTutorProfileInformationRequest request, int userId)
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



