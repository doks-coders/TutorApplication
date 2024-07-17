namespace TutorApplication.SharedModels.Models
{

	public class SessionInfo
	{
		public Guid RecieverId { get; set; }
		public string IsGroup { get; set; }
		public string SenderEmail { get; set; }
		public Guid SenderId { get; set; }
		public Guid CourseGroupId { get; set; }
		public string? ConnectionId { get; set; }
		public Guid? UserId { get; set; }
	}
}
