using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Entities
{
	public class CourseStudent
	{
		public int Id { get; set; }

		public int CourseId { get; set; }

		[ForeignKey(nameof(CourseId))]
		[DeleteBehavior(DeleteBehavior.NoAction)]
		public Course Course { get; set; }

		public int StudentId { get; set; }

		[ForeignKey(nameof(StudentId))]
		[DeleteBehavior(DeleteBehavior.NoAction)]
		public ApplicationUser Student { get; set; }



		public int TutorId { get; set; }

		[ForeignKey(nameof(TutorId))]
		[DeleteBehavior(DeleteBehavior.NoAction)]
		public ApplicationUser Tutor { get; set; }
	}
}
