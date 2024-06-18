namespace TutorApplication.SharedModels.Requests
{
	public class CourseGroupMessageRequest
	{
		public Guid CourseGroupId { get; set; }
		public string Content { get; set; }
	}
}
