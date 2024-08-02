using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorApplication.SharedModels.Entities
{
	public class Message : BaseEntity
	{
		public string Content { get; set; }

		public Guid? SenderId { get; set; }

		[ForeignKey(nameof(SenderId))]
		[DeleteBehavior(DeleteBehavior.NoAction)]
		public ApplicationUser Sender { get; set; }

		
		public Guid? RecieverId { get; set; }

		[ForeignKey(nameof(RecieverId))]
		[DeleteBehavior(DeleteBehavior.NoAction)]
		public ApplicationUser Reciever { get; set; }

		public bool isCourseGroup { get; set; } = false;

		public Guid CourseId { get; set; }

		public bool isDeleted { get; set; } = false;

		public List<Photo>? Photos { get; set; }

		public Guid? QuizId { get; set; }
		[ForeignKey(nameof(QuizId))]
		[DeleteBehavior(DeleteBehavior.NoAction)]
		public Quiz Quiz { get; set; }


	}
}
