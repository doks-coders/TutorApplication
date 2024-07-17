using TutorApplication.SharedModels.Models;

namespace TutorApplication.SharedModels.Responses
{
	public class CourseResponse
	{
		public Guid Id { get; set; }
		public string ImageUrl { get; set; }
		public Guid NavigationId { get; set; }
		public string CourseTitle { get; set; }
		public string Tags { get; set; }
		public string About { get; set; }
		public int Price { get; set; }
		public string Currency { get; set; }
		public int Weeks { get; set; }
		public Dictionary<string, List<MemoResponse>> WeeklyOutline { get; set; }
		public bool hasEnrolled { get; set; } = true;
		public int NumberOfBookedStudents { get; set; }
		public bool? isAdmin { get; set; } = false;
		public List<Memo>? Memos { get; set; }
		public string? TutorName { get; set; }
		public string? TutorImageUrl { get; set; }
		public string? TutorTitle { get; set; }

		public PhotoResponse? Photo { get; set; }
	}


	public class UnfinishedCourseResponse
	{
		public Guid Id { get; set; }
		public string ImageUrl { get; set; }
		public string CourseTitle { get; set; }
		public string About { get; set; }
		public int Price { get; set; }
		public string? CourseStep { get; set; } = "";
		public string Currency { get; set; }
		public bool? isAdmin { get; set; } = false;
		public string Tags { get; set; }
		public string Memos { get; set; }
		public PhotoResponse? Photo { get; set; }
	}
}
