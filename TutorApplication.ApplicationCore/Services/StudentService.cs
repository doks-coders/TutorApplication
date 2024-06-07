using MediatR;
using System.Text.Json;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.ApplicationCore.Utils;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Enums;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses;


namespace TutorApplication.ApplicationCore.Services
{
	public class StudentService:IStudentService
    {
        private readonly IMediator _mediator;
		private readonly IUnitOfWork _unitOfWork;
        public StudentService(IMediator mediator,IUnitOfWork unitOfWork) 
        {
            _mediator = mediator;
			_unitOfWork = unitOfWork;
        }

		public Task<ResponseModel> GetStudent(int studentId)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseModel> GetStudentExtended(int studentId)
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
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Id = user.Id,
				Title = user.Title
			};
			var item = await _unitOfWork.CourseStudents.GetItems(u => u.StudentId == studentId,includeProperties: "Tutor,Course");
			
			response.Courses = item.Select(e => new CourseResponse()
			{
				Id = e.Course.Id,
				NavigationId= e.Course.NavigationId,
				About = e.Course.About,
				CourseTitle = e.Course.CourseTitle,
				Price = e.Course.Price,
				Weeks = JsonSerializer.Deserialize<IEnumerable<Memo>>(e.Course.Memos, options)
					.ConvertMemosToWeekChapters().Count()
			}).ToList();

			var tutors =  item.Select(e => new TutorResponse()
			{
				Id = e.Tutor.Id,
				NavigationId=e.Tutor.NavigationId,
				About = e.Tutor.About,
				Title = e.Tutor.Title,
				Email = e.Tutor.Email,
				FirstName = e.Tutor.FirstName,
				LastName = e.Tutor.LastName,
			}).GroupBy(e=>e.Id).Select(g => g.First()).ToList();

		

			response.Tutors = tutors.ToList();


			return ResponseModel.Send(response);
			
		}
		

		public async Task<ResponseModel> JoinCourse(int courseId, int studentId)
		{
			var checkCourseStudent = await _unitOfWork.CourseStudents.GetItem(u => u.CourseId == courseId && u.StudentId == studentId);
			if (checkCourseStudent != null) return ResponseModel.Send("You have joined this course already");
			var course = await _unitOfWork.Courses.GetItem(u => u.Id == courseId);
			if (course == null) throw new CustomException(ErrorCodes.UserDoesNotExist);
			var courseStudent = new CourseStudent()
			{
				CourseId = courseId,
				StudentId = studentId,
				TutorId = course.TutorId,
			};
			if(await _unitOfWork.CourseStudents.AddItem(courseStudent))
			{
				if (await _unitOfWork.SaveChanges())
				{
					return ResponseModel.Send("Registered Successfully");
				}
				throw new CustomException(ErrorCodes.ErrorWhileSaving);
			}
			throw new CustomException(ErrorCodes.ErrorWhileAdding);
		}

		public async Task<ResponseModel> UpdateStudentProfileInfo(UpdateStudentProfileInformationRequest request, int userId)
		{
			var user = await _unitOfWork.Users.GetItem(u => u.Id == userId);
			if (user == null) throw new CustomException(ErrorCodes.UserDoesNotExist);
			user.FirstName = request.FirstName;
			user.LastName = request.LastName;
			user.FullName = request.LastName + " " + request.FirstName;
			user.FullNameBackwards = request.FirstName + " " + request.LastName;
			user.Title = request.Title;
			user.About = request.About;
			user.isProfileUpdated = true;
	
			if(await _unitOfWork.SaveChanges())
			{
				return ResponseModel.Send(user);
			}
			throw new CustomException(ErrorCodes.ErrorWhileSaving);
		}
	}
}
