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
using TutorApplication.ApplicationCore.Extensions;
using System.Security.Claims;
using TutorApplication.SharedModels.Extensions;
using TutorApplication.ApplicationCore.SignalR.Services;


namespace TutorApplication.ApplicationCore.Services
{
	public class StudentService : IStudentService
	{
		private readonly IMediator _mediator;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMessageHubServices _messageHubServices;
		private readonly IHubServices _hubServices;

		public StudentService(IMediator mediator, IUnitOfWork unitOfWork, IMessageHubServices messageHubServices, IHubServices hubServices)
		{
			_mediator = mediator;
			_unitOfWork = unitOfWork;
			_messageHubServices = messageHubServices;
			_hubServices = hubServices;
		}

		public Task<ResponseModel> GetStudent(Guid studentId)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseModel> GetStudentExtended(Guid studentId)
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
				Interests = user.Interests,
				ImageUrl = user.ImageUrl,
				FirstName = user.FirstName,
				LastName = user.LastName,
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


			return ResponseModel.Send(response);

		}


		public async Task<ResponseModel> JoinCourse(Guid courseId, Guid studentId,ClaimsPrincipal user)
		{
			var checkCourseStudent = await _unitOfWork.CourseStudents.GetItem(u => u.CourseId == courseId && u.StudentId == studentId);
			if (checkCourseStudent != null) return ResponseModel.Send("You have joined this course already");
			var course = await _unitOfWork.Courses.GetItem(u => u.Id == courseId);
			if (course == null) throw new CustomException(ErrorCodes.UserDoesNotExist);
			var courseStudent = new CourseStudent()
			{
				CourseId = courseId,
				StudentId = studentId,
				TutorId =(Guid) course.TutorId,
			};

			var tutor = await _unitOfWork.Users.GetItem(u => u.Id == course.TutorId);
			if (await _unitOfWork.CourseStudents.AddItem(courseStudent))
			{
				if (await _unitOfWork.SaveChanges())
				{
					//Connect To Tutor
					string tutorRecieverName = await _hubServices.GetReceiver("false", course.TutorId, course.Id);
					var directGroupName = HubUtils.GetGroupName(user.GetUserEmail(), tutorRecieverName, "false");

					//Direct Meessaging
					await _messageHubServices.AddUserGroupDirect(directGroupName, user.GetUserEmail(), user.GetUserId(),course.TutorId,course.Id, "false");
					await _messageHubServices.AddUserGroupDirect(directGroupName, tutor.Email, tutor.Id, user.GetUserId(), course.Id,"false");


					//Connect To Group 
					string groupRecieverName = await _hubServices.GetReceiver("true", course.TutorId, course.Id);
					var groupName = HubUtils.GetGroupName(user.GetUserEmail(), groupRecieverName, "true");
					await _messageHubServices.AddUserGroup(groupName, user.GetUserEmail(), user.GetUserId(), course.TutorId, course.Id, "true");
					await _messageHubServices.AddUserGroup(groupName, tutor.Email, tutor.Id, user.GetUserId(), course.Id, "true");




					//Tutor

					return ResponseModel.Send("Registered Successfully");
				}
				throw new CustomException(ErrorCodes.ErrorWhileSaving);
			}
			throw new CustomException(ErrorCodes.ErrorWhileAdding);
		}

		public async Task<ResponseModel> UpdateStudentProfileInfo(UpdateStudentProfileInformationRequest request, Guid userId)
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

			if (await _unitOfWork.SaveChanges())
			{
				return ResponseModel.Send(user);
			}
			throw new CustomException(ErrorCodes.ErrorWhileSaving);
		}
	}
}
