using Azure.Core;
using CloudinaryDotNet.Actions;
using MediatR;
using Microsoft.AspNetCore.Http;
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
using TutorApplication.SharedModels.StaticDefinitions;

namespace TutorApplication.ApplicationCore.Services
{
	public class CourseService : ICourseService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IStudentService _studentService;
		private readonly IPhotoService _photoService;
		public CourseService(IUnitOfWork unitOfWork, IPhotoService photoService, IStudentService studentService)
		{
			_unitOfWork = unitOfWork;
			_photoService = photoService;
			_studentService = studentService;
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

			var course = await _unitOfWork.Courses.GetItem(u => u.Id == id, includeProperties: "Tutor,Students");


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
			(u.CourseTitle.ToLower().Contains(pagination.CourseKeyWord.ToLower()) || pagination.CourseKeyWord == "All") && u.isDetailsCompleted == true;
			// u.About.ToLower().Contains(pagination.CourseKeyWord.ToLower())||
		}

		//Add Extension here to fix repeatability
		public async Task<ResponseModel> SearchCourses(PaginationRequest request)
		{
			var items = await _unitOfWork.Courses.GetPaginationItems(request, Search(request), includeProperties: "Tutor,Students");

			items.Items = ((IEnumerable<Course>)items.Items).ConvertCourseToCourseResponse();

			return ResponseModel.Send(items);
		}


	
		public async Task<ResponseModel> InitialiseCourse(ClaimsPrincipal user)
		{
			Course course = new();
			course.TutorId = user.GetUserId();
			await _unitOfWork.Courses.AddItem(course);
			await _unitOfWork.SaveChanges();
			return ResponseModel.Send(course.Id);
		}

		public async Task<ResponseModel> GetUnfinishedCourse(ClaimsPrincipal user)
		{
			var courses = await _unitOfWork.Courses.GetItems(u=>u.TutorId== user.GetUserId() && u.isDetailsCompleted==false,includeProperties:"Photo");
			var course = courses.OrderByDescending(u=>u.Created).FirstOrDefault();
			return ResponseModel.Send(course!=null? course.ConvertCourseToUnfinishedCourseResponse():null);
		}

		public async Task<ResponseModel> GetUnfinishedCourse(Guid courseId)
		{
			var course = await _unitOfWork.Courses.GetItem(u => u.Id == courseId, includeProperties: "Photo");
			var response = course.ConvertCourseToUnfinishedCourseResponse();
			return ResponseModel.Send(response);
		}


		public class CourseNameRequest
		{
			public string CourseTitle { get; set; }
		}
		
		public async Task<ResponseModel> UpdateCourseName(CourseNameRequest request, Guid courseid)
		{
			var course = await _unitOfWork.Courses.GetItem(u => u.Id == courseid);
			course.CourseTitle = request.CourseTitle;
			course.CourseStep = CourseSetup.CourseAbout;
			await _unitOfWork.SaveChanges();
			return ResponseModel.Send(CourseSetup.CourseAbout);
		}
	
		public class CourseAboutRequest
		{
			public string About { get; set; }
		}
		public async Task<ResponseModel> UpdateCourseAbout(CourseAboutRequest request, Guid courseid)
		{
			var course = await _unitOfWork.Courses.GetItem(u => u.Id == courseid);
			course.About = request.About;
			course.CourseStep = CourseSetup.CourseImage;
			await _unitOfWork.SaveChanges();
			return ResponseModel.Send(CourseSetup.CourseImage);
		}


		public class CourseImageRequest
		{
			public string ImageUrl { get; set; }
		}
		public async Task<ResponseModel> UpdateCourseImage(CourseImageRequest request, Guid courseId)
		{
			var course = await _unitOfWork.Courses.GetItem(u => u.Id == courseId);
			course.ImageUrl = request.ImageUrl;
			course.CourseStep = CourseSetup.CourseTags;
			await _unitOfWork.SaveChanges();	

			return ResponseModel.Send(CourseSetup.CourseTags);
		}

		public class CourseTagsRequest
		{
			public string Tags { get; set; }
		}

	
		public async Task<ResponseModel> UpdateCourseTags(CourseTagsRequest request, Guid courseId)
		{
			var course = await _unitOfWork.Courses.GetItem(u => u.Id == courseId);
			course.Tags = request.Tags;
			course.CourseStep = CourseSetup.CourseSchedule;
			await _unitOfWork.SaveChanges();

			return ResponseModel.Send(CourseSetup.CourseSchedule);
		}

		public class CourseMemoRequest
		{
			public string Memos { get; set; }
		}

		public async Task<ResponseModel> UpdateCourseSchedule(CourseMemoRequest request,Guid courseid)
		{
			var course = await _unitOfWork.Courses.GetItem(u => u.Id == courseid);
			course.Memos = request.Memos;
			course.CourseStep = CourseSetup.CoursePricing;
			await _unitOfWork.SaveChanges();

			return ResponseModel.Send(CourseSetup.CoursePricing);
		}

	

		public class CoursePricingRequest
		{
			public int Price { get; set; }
			public string Currency { get; set; }
		}
		public async Task<ResponseModel> UpdateCoursePricing(CoursePricingRequest request, Guid courseId, ClaimsPrincipal user)
		{
			var course = await _unitOfWork.Courses.GetItem(u => u.Id == courseId);
			course.Price = request.Price;
			course.Currency = request.Currency;
			course.isDetailsCompleted = true;
			await _unitOfWork.SaveChanges();

			await _studentService.JoinCourse(course.Id, course.TutorId, user);

			return ResponseModel.Send();
		}



		

		public async Task<ResponseModel> GetAllClassesForUser(ClaimsPrincipal user,string userMode="student")
		{

			Guid userId = Guid.NewGuid();
			try
			{
				userId = user.GetUserId();


			}
			catch (Exception ex)
			{
				throw new CustomException(ErrorCodes.UserAuthDoesNotExist);
			}

			List<ClassResponse> classes = new();
			if (userMode == "student")
			{
				var courseStudents = await _unitOfWork.CourseStudents.GetOneCourseStudentForStudent(userId);
				var courses = courseStudents.Select(u => u.Course);
				classes = courses.ConvertCourseToClasses().ToList();
			}
			else
			{
				var courses = await _unitOfWork.Courses.GetItems(u=>u.TutorId ==userId && u.isDetailsCompleted==true,includeProperties: "Students,Tutor");
				classes = courses.ConvertCourseToClasses().ToList();
			}
			


			long? timestamp = null;// 1720283128178;
			int days = 7;
			Dictionary<string, List<ClassResponse>> keyValuePairs = new Dictionary<string, List<ClassResponse>>();

			
			DateTime dateTime = timestamp != null ? CourseServiceUtils.GetTodayFromTimeStamp((long)timestamp) : DateTime.Today;

			var dayStart = dateTime;
			var dayEnd = dateTime.AddHours(days * 24);

			//Creates Dictionary with all the days
			Enumerable.Range(0, days).Select((val, index) => (val, index)).ToList().ForEach((val) =>
			{
				keyValuePairs[CourseServiceUtils.ConvertDateTimeToStamp(dayStart.AddDays(val.index)).ToString()] = new();
			});

		
			classes.ToList().ForEach(val =>
			{
				var memoDate = CourseServiceUtils.ConvertDateStringToDate(val.Date);
				string[] time = val.Time.Split(":");
				var hour = int.Parse(time[0]);
				var minute = int.Parse(time[1]);
				var updatedDate = memoDate.AddHours(hour).AddMinutes(minute);

				if (dayStart < updatedDate && dayEnd > updatedDate)
				{
					val.Timestamp = CourseServiceUtils.ConvertDateTimeToStamp(updatedDate).ToString();

					keyValuePairs[CourseServiceUtils.ConvertDateTimeToStamp(
						CourseServiceUtils.GetDayStartFromDate(updatedDate)).ToString()].Add(val);

				}

			});

			return ResponseModel.Send(keyValuePairs);




		}
	}
}
