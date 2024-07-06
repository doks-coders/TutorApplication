using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using TutorApplication.ApplicationCore.Extensions;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.ApplicationCore.Utils;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Enums;
using TutorApplication.SharedModels.Extensions;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses;

namespace TutorApplication.ApplicationCore.Services
{
	public class CourseService : ICourseService
	{
		private readonly IUnitOfWork _unitOfWork;

		public CourseService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<ResponseModel> CreateCourse(CreateCourseRequest request, Guid tutorId)
		{
			var course = request.ConvertCourseRequestToCourse(tutorId);
			if (await _unitOfWork.Courses.AddItem(course))
			{
				if (await _unitOfWork.SaveChanges())
				{
					return ResponseModel.Send("Item Added Successfully", HttpStatusCode.Created);
				}
				throw new CustomException(ErrorCodes.ErrorWhileSaving);
			}
			throw new CustomException(ErrorCodes.ErrorWhileAdding);

		}

		public async Task<ResponseModel> UpdateCourse(CreateCourseRequest request, Guid userId, Guid courseId)
		{
			var course = await _unitOfWork.Courses.GetItem(u => u.Id == courseId);
			if (course == null) throw new CustomException(ErrorCodes.UserDoesNotExist);

			if (course.TutorId != userId) throw new CustomException(ErrorCodes.UserDoesNotExist);

			course.CourseTitle = request.CourseTitle;
			course.Currency = request.Currency;
			course.Memos = request.Memos;
			course.About = request.About;
			course.Price = request.Price;
			course.Created = DateTime.UtcNow;
			if (await _unitOfWork.SaveChanges())
			{
				return ResponseModel.Send("Updated Successfully");
			}
			throw new CustomException(ErrorCodes.ErrorWhileSaving);
		}

		public async Task<ResponseModel> GetCourseInfo(Guid id, ClaimsPrincipal user)
		{

			JsonSerializerOptions options = new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			var course = await _unitOfWork.Courses.GetItem(u => u.Id == id,includeProperties: "Tutor,Students");


			bool isAdmin = false;
			bool hasEnrolled = false;


			var studentsBooked = await _unitOfWork.CourseStudents.GetItems(u => u.CourseId == course.Id);

			try
			{
				var userId = user.GetUserId();
				isAdmin = (course.TutorId == userId);

				var exists = studentsBooked.Where(u => u.CourseId == course.Id && u.StudentId == userId).FirstOrDefault();
				if (exists != null)
				{
					hasEnrolled = true;
				}
			}

			catch (Exception) { }
			var memos = JsonSerializer.Deserialize<IEnumerable<Memo>>(course.Memos, options);
			var response = course.ConvertCourseToCourseExtendedResponse(memos, isAdmin, hasEnrolled);
			return ResponseModel.Send(response);
		}

		private Expression<Func<Course, bool>> Search(PaginationRequest pagination)
		{
			return u =>
			(u.CourseTitle.ToLower().Contains(pagination.CourseKeyWord.ToLower()) || pagination.CourseKeyWord == "All" || string.IsNullOrEmpty(pagination.CourseKeyWord) || u.About.ToLower().Contains(pagination.CourseKeyWord.ToLower()));

		}

		//Add Extension here to fix repeatability
		public async Task<ResponseModel> SearchCourses(PaginationRequest request)
		{
			var items = await _unitOfWork.Courses.GetPaginationItems(request, Search(request),includeProperties: "Tutor,Students");

			items.Items = ((IEnumerable<Course>)items.Items).ConvertCourseToCourseResponse();

			return ResponseModel.Send(items);
		}
	}
}
