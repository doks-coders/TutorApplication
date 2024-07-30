using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface ISearchService
	{
		Task<ResponseModel> SearchUsers(PaginationRequest request);
		Task<ResponseModel> SearchMyUsers(PaginationRequest request, ClaimsPrincipal user);
		Task<ResponseModel> SearchUsers(PaginationRequest request, Guid userId);

		Task<ResponseModel> SearchCourses(PaginationRequest request);
		Task<ResponseModel> SearchCourses(PaginationRequest request, Guid userId);
		Task<ResponseModel> SearchMyCourses(PaginationRequest request, ClaimsPrincipal user);
	}
}
