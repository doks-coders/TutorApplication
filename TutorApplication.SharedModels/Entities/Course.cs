using System.ComponentModel.DataAnnotations.Schema;

namespace TutorApplication.SharedModels.Entities
{
	public class Course : BaseEntity
	{
		public string CourseTitle { get; set; }
		public string About { get; set; }
		public int Price { get; set; }
		public string Currency { get; set; }
		public string Memos { get; set; }
		public int TutorId { get; set; }
		[ForeignKey(nameof(TutorId))]
		public ApplicationUser Tutor { get; set; }
	}
}
