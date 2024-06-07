namespace TutorApplication.SharedModels.Models
{

	public class SessionInfo
	{
		public string RecieverId { get; set; }
		public string IsGroup { get; set; }
		public string SenderEmail { get; set; }
		public int SenderId { get; set; }
		public string CourseGroupId { get; set; }
	}
}
