namespace TutorApplication.SharedModels.Responses
{
	public class TutorExtendedResponse
	{
		public Guid Id { get; set; }
		public Guid NavigationId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Title { get; set; }
		public string About { get; set; }
		public string Email { get; set; }
		public List<CourseResponse> Courses { get; set; }
		public List<StudentResponse> Students { get; set; }
	}


}
