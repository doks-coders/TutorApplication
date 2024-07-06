using Azure.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TutorApplication.ApplicationCore.Utils;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses;


namespace TutorApplication.ApplicationCore.Extensions
{
	public static class CourseExtensions
	{
		public static  CourseResponse ConvertCourseToCourseExtendedResponse(this Course course, 
			IEnumerable<Memo> memos,
			bool isAdmin,bool hasEnrolled)
		{
			return new CourseResponse()
			{
				Id = course.Id,
				NavigationId = course.NavigationId,
				About = course.About,
				CourseTitle = course.CourseTitle,
				Currency = course.Currency,
				Price = course.Price,
				isAdmin = isAdmin,
				hasEnrolled = hasEnrolled,
				WeeklyOutline = memos.ConvertMemosToWeekChapters(),
				Memos = memos.ToList(),
				Weeks = memos
					.ConvertMemosToWeekChapters().Count(),
				NumberOfBookedStudents = course.Students.Count(),
				TutorName = course.Tutor.LastName +" "+course.Tutor.FirstName,
				TutorTitle = course.Tutor.Title
			};
		}


		public static List<CourseResponse> ConvertCourseToCourseResponse(this IEnumerable<Course> course)
		{
			JsonSerializerOptions options = new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			return course.Select(e => new CourseResponse()
			{
				Id = e.Id,
				NavigationId = e.NavigationId,
				CourseTitle = e.CourseTitle,
				Currency = e.Currency,
				About = e.About,
				Price = e.Price,
				Weeks = JsonSerializer.Deserialize<IEnumerable<Memo>>(e.Memos, options)
					.ConvertMemosToWeekChapters().Count(),
				TutorName = e.Tutor.LastName + " " + e.Tutor.FirstName,
				NumberOfBookedStudents = e.Students.Count()


			}).ToList();
		}
		public static List<CourseResponse> ConvertCourseToCourseResponse(this IEnumerable<CourseStudent> courseStudents)
		{
			JsonSerializerOptions options = new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			return courseStudents.Select(e => new CourseResponse()
			{
				Id = e.Course.Id,
				NavigationId = e.Course.NavigationId,
				CourseTitle = e.Course.CourseTitle,
				Currency = e.Course.Currency,
				About = e.Course.About,
				Price = e.Course.Price,
				Weeks = JsonSerializer.Deserialize<IEnumerable<Memo>>(e.Course.Memos, options)
					.ConvertMemosToWeekChapters().Count(),
				TutorName = e.Course.Tutor.LastName + " " + e.Course.Tutor.FirstName,
				NumberOfBookedStudents = e.Course.Students.Count()


			}).ToList();
		}

		public static Course ConvertCourseRequestToCourse(this CreateCourseRequest request, Guid tutorId)
		{
			return new Course()
			{
				TutorId = tutorId,
				CourseTitle = request.CourseTitle,
				Currency = request.Currency,
				Memos = request.Memos,
				About = request.About,
				Price = request.Price



			};

		}
	}
}
