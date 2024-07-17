using Azure.Core;
using CloudinaryDotNet;
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
				ImageUrl = course.ImageUrl,
				NavigationId = course.NavigationId,
				About = course.About,
				Tags = course.Tags,
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
				TutorImageUrl = course.Tutor.ImageUrl,
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
				ImageUrl = e.ImageUrl,
				CourseTitle = e.CourseTitle,
				Currency = e.Currency,
				About = e.About,
				Price = e.Price,
				Weeks = JsonSerializer.Deserialize<IEnumerable<Memo>>(e.Memos, options)
					.ConvertMemosToWeekChapters().Count(),
				TutorImageUrl = e.Tutor.ImageUrl,
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
				ImageUrl = e.Course.ImageUrl,
				Currency = e.Course.Currency,

				About = e.Course.About,
				Price = e.Course.Price,
				Weeks = JsonSerializer.Deserialize<IEnumerable<Memo>>(e.Course.Memos, options)
					.ConvertMemosToWeekChapters().Count(),
				TutorImageUrl = e.Course.Tutor.ImageUrl,
				TutorName = e.Course.Tutor.LastName + " " + e.Course.Tutor.FirstName,
				NumberOfBookedStudents = e.Course.Students.Count()


			}).ToList();
		}

		public static UnfinishedCourseResponse ConvertCourseToUnfinishedCourseResponse(this Course course)
		{
			JsonSerializerOptions options = new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

				return new UnfinishedCourseResponse()
			{
				Id = course.Id,
				CourseTitle = course.CourseTitle,
				Currency = course.Currency,
				About = course.About,
				Price = course.Price,
				Memos = course.Memos,
				Tags = course.Tags,
				CourseStep = course.CourseStep,

				ImageUrl = course.ImageUrl,
				Photo = course.Photo != null ? course.Photo.ConvertPhotoToPhotoResponse(): null

		};
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


		public static IEnumerable<ClassResponse> ConvertCourseToClasses(this IEnumerable<Course> courses)
		{
			var options = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true,
			};

			List<ClassResponse> classes = new List<ClassResponse>();
			courses.ToList().ForEach(course =>
			{
				var memos = JsonSerializer.Deserialize<IEnumerable<Memo>>(course.Memos, options);

				var cl = memos.Select(u => new ClassResponse()
				{
					CourseId = course.Id,
					ImageUrl = course.ImageUrl,
					CourseNavigationId = course.NavigationId,
					CourseTitle = course.CourseTitle,
					BookInfo = u.BookInfo,
					Date = u.Date,
					Time = u.Time,
					Type = u.Type,
					NumberOfBookedStudents = course.Students.Count(),

					TutorName = course.Tutor.FullName
				});
				classes.AddRange(cl);

				// Timestamp in milliseconds

			});
			return classes;
		}
	}
}
