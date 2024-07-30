using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Entities
{
	

	public class Quiz:BaseEntity
	{
		public Guid? CourseId { get; set; }
		[ForeignKey(nameof(CourseId))]
		[DeleteBehavior(DeleteBehavior.NoAction)]
		public Course Course { get; set; }
		public string QuizName { get; set; }
		public string QuizQuestions { get; set; }
	}
}
