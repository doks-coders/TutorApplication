using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using TutorApplication.ApplicationCore.Services;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.Controllers
{
	public class QuizController : BaseController
	{
		private readonly IQuizService _quizService;

		public QuizController(IQuizService quizService)
		{
			_quizService = quizService;
		}

		[HttpPost("create-quiz")]
		public async Task<ActionResult>  CreateQuiz(CreateQuizRequest request)
		{
			return await _quizService.CreateQuiz(request);
			
		}
		[HttpPost("update-quiz")]
		public async Task<ActionResult> UpdateQuiz(UpdateQuizRequest request)
		{
			return await _quizService.UpdateQuiz(request);
		}

		


		[HttpGet("get-quiz/{quizId}")]
		public async Task<ActionResult> GetQuiz(Guid quizId)
		{
			return await _quizService.GetQuiz(quizId);
		}

		[HttpGet("get-complete-quiz/{quizId}")]
		public async Task<ActionResult> GetCompleteQuiz(Guid quizId)
		{
			return await _quizService.GetCompleteQuiz(quizId,User);
		}

		[HttpGet("get-course-quizs/{courseId}")]
		public async Task<ActionResult> GetQuizForCourse(Guid courseId)
		{
			return await _quizService.GetQuizForCourse(courseId);
		}

		[HttpGet("get-quiz-question")]
		public async Task<ActionResult> GetQuizQuestion([FromQuery] QuizQuestionRequest request)
		{
			return await _quizService.GetQuizQuestion(request);
		}
	}
}
