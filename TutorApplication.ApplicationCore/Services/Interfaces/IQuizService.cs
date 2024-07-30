using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TutorApplication.ApplicationCore.Services.QuizService;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	public interface IQuizService
	{
		Task<ResponseModel> GetQuiz(Guid quizId);
		Task<ResponseModel> CreateQuiz(CreateQuizRequest request);
		Task<ResponseModel> GetQuizQuestion(QuizQuestionRequest request);
		Task<ResponseModel> GetQuizForCourse(Guid courseId);
	}
}
