using TutorApplication.SharedModels.Responses;

namespace TutorApplication.SharedModels.Requests
{
	public class MessageRequest
	{
		public bool isGroup { get; set; } = false;

		public Guid? RecieverId { get; set; }
		public List<PhotoResponse>? Photos { get; set; }	
		public Guid? CourseGroupId { get; set; }
		public string Content { get; set; }
		public Guid? QuizId { get; set; }
	}
}
