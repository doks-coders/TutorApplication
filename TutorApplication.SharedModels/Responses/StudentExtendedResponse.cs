namespace TutorApplication.SharedModels.Responses
{
	public class StudentExtendedResponse
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Title { get; set; }
		public string About { get; set; }
		public string Email { get; set; }
		public List<CourseResponse> Courses { get; set; } = new();
		public List<TutorResponse> Tutors { get; set; } = new();

	}
}
