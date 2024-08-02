using TutorApplication.SharedModels.Requests;

namespace TutorApplication.SharedModels.Responses.Messages
{
	public class MessageResponse
	{
		public Guid Id { get; set; }
		public string Content { get; set; }
		public string SenderName { get; set; }
		public string Mode { get; set; } = "chat";
		public DateTime Created { get; set; }
		public Guid SenderId { get; set; }
		public List<PhotoResponse>? Photos{ get; set; }
		public QuizResponse? Quiz { get; set; }
	}
}
