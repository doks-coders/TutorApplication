using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Requests
{
	public class CreateQuizRequest
	{
		public string QuizName { get; set; }
		public string QuizQuestions { get; set; }
		public Guid? CourseId { get; set; }
	}

	public class UpdateQuizRequest
	{
		public Guid Id { get; set; }
		public string QuizName { get; set; }
		public string QuizQuestions { get; set; }
	}

	public class QuizQuestionRequest
	{
		public int QuestionIndex { get; set; }
		public string QuizId { get; set; }
	}

	public class QuizResponse
	{
		public string QuizName { get; set; }
		public Guid Id { get; set; }
	}

}
