using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorApplication.SharedModels.Entities
{
	public class Course : BaseEntity
	{
		public string CourseTitle { get; set; } = "";
		public string About { get; set; } = "";
		public int Price { get; set; } = 0;
		public string Currency { get; set; } = "";
		public string Memos { get; set; } = "[]";

		public string? ImageUrl { get; set; } = "";

		public string? CourseStep { get; set; } = "";

		public Guid? PhotoId { get; set; }
	
		[ForeignKey(nameof(PhotoId))]
		[DeleteBehavior(DeleteBehavior.NoAction)]
		public Photo Photo { get; set; }

		public string? Tags { get; set; } = "";
		public bool? isDetailsCompleted { get; set; } = false;

		public Guid TutorId { get; set; }
		[ForeignKey(nameof(TutorId))]
		public ApplicationUser Tutor { get; set; }

		public List<CourseStudent> Students { get; set; } = new List<CourseStudent>();
	}
}
