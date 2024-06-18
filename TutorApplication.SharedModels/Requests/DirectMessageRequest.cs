namespace TutorApplication.SharedModels.Requests
{
	public class DirectMessageRequest
	{
		public Guid RecieverId { get; set; }
		public string Content { get; set; }
	}
}
