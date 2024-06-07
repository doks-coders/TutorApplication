namespace TutorApplication.SharedModels.Entities
{
	public class Message : BaseEntity
	{
		public string Content { get; set; }

		public int? SenderId { get; set; }

		public ApplicationUser Sender { get; set; }

		public int? RecieverId { get; set; }

		public ApplicationUser Reciever { get; set; }

		public bool isCourseGroup { get; set; } = false;

		public int CourseId { get; set; }

		public bool isDeleted { get; set; } = false;

	}
}
