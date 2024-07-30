using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TutorApplication.ApplicationCore.Extensions;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Constants;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Extensions;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses;

namespace TutorApplication.ApplicationCore.Services
{
	public class SearchService:ISearchService
	{
		private readonly IUnitOfWork _unitOfWork;

		public SearchService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		public async Task<ResponseModel> SearchCourses()
		{
			return ResponseModel.Send();
		}

		private Expression<Func<ApplicationUser, bool>> UserSearch(PaginationRequest request)
		{
			return u =>
			   (request.NameKeyword == "All" || u.FullName.ToLower().Contains(request.NameKeyword.ToLower()) || u.FullNameBackwards.ToLower().Contains(request.NameKeyword.ToLower()))
			   &&
			   u.AccountType == RoleConstants.Tutor
			   && u.isProfileUpdated == true;
		}

		private Expression<Func<CourseStudent, bool>> TutorSearch(PaginationRequest request)
		{
			return u =>
			   (request.NameKeyword == "All" || u.Tutor.FullName.ToLower().Contains(request.NameKeyword.ToLower()) || u.Tutor.FullNameBackwards.ToLower().Contains(request.NameKeyword.ToLower()))
			   && u.Tutor.AccountType == RoleConstants.Tutor
			   && u.Tutor.isProfileUpdated == true;
		}

		private Expression<Func<CourseStudent, bool>> StudentSearch(PaginationRequest request)
		{
			return u =>
			   (request.NameKeyword == "All" || u.Student.FullName.ToLower().Contains(request.NameKeyword.ToLower()) || u.Student.FullNameBackwards.ToLower().Contains(request.NameKeyword.ToLower()))
			   && u.Student.AccountType == RoleConstants.Student
			   && u.Student.isProfileUpdated == true;
		}

		private Expression<Func<CourseStudent, bool>> StudentToTutorSearch(PaginationRequest request)
		{
			return u =>
			   (request.NameKeyword == "All" || u.Tutor.FullName.ToLower().Contains(request.NameKeyword.ToLower()) || u.Tutor.FullNameBackwards.ToLower().Contains(request.NameKeyword.ToLower()))
			   &&
			   u.Tutor.AccountType == RoleConstants.Tutor
			   &&
			   u.StudentId == new Guid(request.UserId)

			   && u.Tutor.isProfileUpdated == true;
		}

		private Expression<Func<CourseStudent, bool>> TutorToStudentsSearch(PaginationRequest request)
		{
			return u =>
			   (request.NameKeyword == "All" || u.Student.FullName.ToLower().Contains(request.NameKeyword.ToLower()) || u.Student.FullNameBackwards.ToLower().Contains(request.NameKeyword.ToLower()))
			   &&
			   u.Student.AccountType == RoleConstants.Student
			   &&
			   u.TutorId == new Guid(request.UserId)
			   && u.Student.isProfileUpdated == true;
		}

		public async Task<ResponseModel> SearchUsers(PaginationRequest request)
		{
			PaginationResponse response;
			if (request.AccountType == RoleConstants.Tutor)
			{
				response = await _unitOfWork.CourseStudents.GetTutorsPaginationItems(request, TutorSearch(request));
			}
			else
			{
				response = await _unitOfWork.CourseStudents.GetStudentsPaginationItems(request, StudentSearch(request));

			}

			response.Items = ((IEnumerable<ApplicationUser>)response.Items).Select(e => new UserResponse()
			{
				Id = e.Id,
				FirstName = e.FirstName,
				LastName = e.LastName,
				About = e.About,
				Title = e.Title,
				Email = e.Email,
				ImageUrl = e.ImageUrl
			}).ToList();

			return ResponseModel.Send(response);
		}


		public async Task<ResponseModel> SearchUsers(PaginationRequest request, Guid userId)
		{
			PaginationResponse response;
			request.UserId = userId.ToString();
			if (request.AccountType == RoleConstants.Tutor)
			{
				response = await _unitOfWork.CourseStudents.GetTutorsPaginationItems(request, TutorSearch(request));
			}
			else
			{
				response = await _unitOfWork.CourseStudents.GetStudentsPaginationItems(request, StudentSearch(request));

			}

			response.Items = ((IEnumerable<ApplicationUser>)response.Items).Select(e => new UserResponse()
			{
				Id = e.Id,
				FirstName = e.FirstName,
				LastName = e.LastName,
				About = e.About,
				Title = e.Title,
				Email = e.Email,
				ImageUrl = e.ImageUrl
			}).ToList();

			return ResponseModel.Send(response);
		}


		public async Task<ResponseModel> SearchMyUsers(PaginationRequest request,ClaimsPrincipal user)
		{
			PaginationResponse response;
			request.UserId = user.GetUserId().ToString();
			if (request.AccountType == RoleConstants.Tutor)
			{
				response = await _unitOfWork.CourseStudents.GetStudentsPaginationItems(request, TutorToStudentsSearch(request)) ;
			}
			else
			{
				response = await _unitOfWork.CourseStudents.GetTutorsPaginationItems(request, StudentToTutorSearch(request));
			}

			response.Items = ((IEnumerable<ApplicationUser>)response.Items).Select(e => new UserResponse()
			{
				Id = e.Id,
				FirstName = e.FirstName,
				LastName = e.LastName,
				About = e.About,
				Title = e.Title,
				Email = e.Email,
				ImageUrl = e.ImageUrl
			}).ToList();

			return ResponseModel.Send(response);
		}



		//Courses Search

		private Expression<Func<CourseStudent, bool>> CoursesSearch(PaginationRequest request)
		{
			return u =>
			  	(u.Course.CourseTitle.ToLower().Contains(request.CourseKeyWord.ToLower()) || request.CourseKeyWord == "All")
				&& u.Course.isDetailsCompleted == true; 
		}

		private Expression<Func<CourseStudent, bool>> TutorToCoursesSearch(PaginationRequest request)
		{
			return u =>
			  	(u.Course.CourseTitle.ToLower().Contains(request.CourseKeyWord.ToLower()) || request.CourseKeyWord == "All") 
				&& u.Course.isDetailsCompleted == true
				&& u.TutorId == new Guid(request.UserId);
		}

		private Expression<Func<CourseStudent, bool>> StudentToCoursesSearch(PaginationRequest request)
		{
			return u =>
			  	(u.Course.CourseTitle.ToLower().Contains(request.CourseKeyWord.ToLower()) || request.CourseKeyWord == "All")
				&& u.Course.isDetailsCompleted == true
				&& u.StudentId == new Guid(request.UserId);
		}


		public async Task<ResponseModel> SearchCourses(PaginationRequest request)
		{
			PaginationResponse response;
		
			response = await _unitOfWork.CourseStudents.GetCoursePaginationItems(request, CoursesSearch(request));

			response.Items = ((IEnumerable<Course>)response.Items).ConvertCourseToCourseResponse();


			return ResponseModel.Send(response);
		}

		public async Task<ResponseModel> SearchCourses(PaginationRequest request, Guid userId)
		{
			PaginationResponse response;
			request.UserId = userId.ToString();
			if (request.AccountType == RoleConstants.Tutor)
			{
				response = await _unitOfWork.CourseStudents.GetCoursePaginationItems(request, TutorToCoursesSearch(request));
			}
			else
			{
				response = await _unitOfWork.CourseStudents.GetCoursePaginationItems(request, StudentToCoursesSearch(request));

			}

			response.Items = ((IEnumerable<Course>)response.Items).ConvertCourseToCourseResponse();


			return ResponseModel.Send(response);
		}

		public async Task<ResponseModel> SearchMyCourses(PaginationRequest request,ClaimsPrincipal user)
		{
			PaginationResponse response;
			request.UserId = user.GetUserId().ToString();
			if (request.AccountType == RoleConstants.Tutor)
			{
				response = await _unitOfWork.CourseStudents.GetCoursePaginationItems(request, TutorToCoursesSearch(request));
			}
			else
			{
				response = await _unitOfWork.CourseStudents.GetCoursePaginationItems(request, StudentToCoursesSearch(request));
			}

			response.Items = ((IEnumerable<Course>)response.Items).ConvertCourseToCourseResponse();

			return ResponseModel.Send(response);
		}


	}
}


/*
	public async Task<ResponseModel> SearchMyTutors(PaginationRequest request,ClaimsPrincipal user)
	{
		var items = await _unitOfWork.Users.GetPaginationItems(request, UserSearch(request));
		items.Items = ((IEnumerable<ApplicationUser>)items.Items).Select(e => new UserResponse()
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

	*/