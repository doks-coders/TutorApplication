namespace TutorApplication.SharedModels.Entities
{
	public class Message : BaseEntity
	{
		public string Content { get; set; }

		public Guid? SenderId { get; set; }

		public ApplicationUser Sender { get; set; }

		public Guid? RecieverId { get; set; }

		public ApplicationUser Reciever { get; set; }

		public bool isCourseGroup { get; set; } = false;

		public Guid CourseId { get; set; }

		public bool isDeleted { get; set; } = false;

	}
}
