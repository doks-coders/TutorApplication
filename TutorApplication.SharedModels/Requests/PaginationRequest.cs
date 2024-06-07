namespace TutorApplication.SharedModels.Requests
{

	public class PaginationRequest
	{
		public int PageNumber { get; set; } = 1;
		public int PageLimit { get; set; } = 3;
		public string TutorName { get; set; } = "";
		public string CourseKeyWord { get; set; } = "";
	}
}
