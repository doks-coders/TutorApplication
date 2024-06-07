namespace TutorApplication.SharedModels.Requests
{
	public class DirectMessageRequest
	{
		public int RecieverId { get; set; }
		public string Content { get; set; }
	}
}
