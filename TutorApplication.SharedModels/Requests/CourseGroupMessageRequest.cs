using TutorApplication.SharedModels.Responses;

namespace TutorApplication.SharedModels.Requests
{
	public class CourseGroupMessageRequest
	{
		public Guid CourseGroupId { get; set; }
		public Guid? QuizId { get; set; }
		public string Content { get; set; }
		public List<PhotoResponse>? Photos { get; set; }	
	}
}
