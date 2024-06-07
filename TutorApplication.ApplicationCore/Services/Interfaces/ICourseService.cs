﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface ICourseService
	{
		Task<ResponseModel> CreateCourse(CreateCourseRequest request, int tutorId);
		Task<ResponseModel> UpdateCourse(CreateCourseRequest request, int userId, int courseId);
		Task<ResponseModel> GetCourseInfo(int id, ClaimsPrincipal user);
		Task<ResponseModel> SearchCourses(PaginationRequest request);
	}
}