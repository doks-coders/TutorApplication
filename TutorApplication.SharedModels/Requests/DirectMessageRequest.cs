using TutorApplication.SharedModels.Responses;

namespace TutorApplication.SharedModels.Requests
{
	public class DirectMessageRequest
	{
		public Guid RecieverId { get; set; }
		public string Content { get; set; }
		public List<PhotoResponse>? Photos { get; set; }
	}
}
