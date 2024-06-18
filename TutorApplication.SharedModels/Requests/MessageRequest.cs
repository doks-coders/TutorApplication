namespace TutorApplication.SharedModels.Requests
{
	public class MessageRequest
	{
		public bool isGroup { get; set; } = false;

		public Guid? RecieverId { get; set; }

		public Guid? CourseGroupId { get; set; }
		public string Content { get; set; }
	}
}
