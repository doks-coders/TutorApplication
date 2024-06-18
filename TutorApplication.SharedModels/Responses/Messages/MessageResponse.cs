namespace TutorApplication.SharedModels.Responses.Messages
{
	public class MessageResponse
	{
		public Guid Id { get; set; }
		public string Content { get; set; }
		public DateTime Created { get; set; }
		public Guid SenderId { get; set; }
	}
}
