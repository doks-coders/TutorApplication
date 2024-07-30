using Microsoft.AspNetCore.Mvc;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.Controllers
{
	public class SearchController : BaseController
	{
		private readonly ISearchService _searchService;

		public SearchController(ISearchService searchService)
		{
			_searchService = searchService;
		}

		[HttpGet("search-users")]
		public async Task<ActionResult> SearchUsers([FromQuery]PaginationRequest request)
		{
			return await _searchService.SearchUsers(request);
		}

		[HttpGet("search-my-users")]
		public async Task<ActionResult> SearchMyUsers([FromQuery] PaginationRequest request)
		{
			return await _searchService.SearchMyUsers(request,User);
		}

		[HttpGet("search-my-courses")]
		public async Task<ActionResult> SearchMyCourses([FromQuery] PaginationRequest request)
		{
			return await _searchService.SearchMyCourses(request, User);
		}

		[HttpGet("search-courses-by-id/{userId}")]
		public async Task<ActionResult> SearchCourses([FromQuery] PaginationRequest request,Guid userId)
		{
			return await _searchService.SearchCourses(request, userId);
		}



	}
}
