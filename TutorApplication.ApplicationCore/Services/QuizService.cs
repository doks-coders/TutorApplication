using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Enums;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.ApplicationCore.Services
{
	

	public class QuizService:IQuizService
	{
		private readonly IUnitOfWork _unitOfWork;

		public QuizService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
	
		public async Task<ResponseModel> CreateQuiz(CreateQuizRequest request)
		{
			var quiz = new Quiz()
			{
				CourseId = request.CourseId,
				QuizName = request.QuizName,
				QuizQuestions = request.QuizQuestions
			};
			if(await _unitOfWork.Quizs.AddItem(quiz))
			{
				await _unitOfWork.SaveChanges();
				return ResponseModel.Send();
			}
			throw new CustomException(ErrorCodes.ErrorWhileSaving);
		}

		public async Task<ResponseModel> UpdateQuiz(UpdateQuizRequest request)
		{
			var quiz = await _unitOfWork.Quizs.GetItem(u => u.Id == request.Id);
			quiz.QuizQuestions = request.QuizQuestions;
			quiz.QuizName = request.QuizName;
			await _unitOfWork.SaveChanges();
			return ResponseModel.Send("Update Completed Successfully");
			
		}

		public async Task<ResponseModel> GetCompleteQuiz(Guid quizId, ClaimsPrincipal user)
		{
			var quiz = await _unitOfWork.Quizs.GetItem(u => u.Id == quizId);
			if (quiz == null) throw new CustomException("Quiz does not exist");
			return ResponseModel.Send(quiz);
		}

		//index and quizId

		public async Task<ResponseModel> GetQuiz(Guid quizId)
		{
			JsonSerializerOptions options = new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
			var quiz = await _unitOfWork.Quizs.GetItem(u => u.Id == quizId);
			if (quiz == null) throw new CustomException("Quiz does not exist");

			var quizQuestions = JsonSerializer.Deserialize<List<QuizQuestionResponse>>(quiz.QuizQuestions, options);
			quiz.QuizQuestions = JsonSerializer.Serialize(quizQuestions,options);

			return ResponseModel.Send(quiz);
		}

		public async Task<ResponseModel> GetQuizForCourse(Guid courseId)
		{
			var quiz = await _unitOfWork.Quizs.GetItems(u => u.CourseId == courseId);
			if (quiz == null) throw new CustomException("Quiz does not exist");
			return ResponseModel.Send(quiz);
		}


		public async Task<ResponseModel> GetQuizQuestion(QuizQuestionRequest request)
		{
			
			var quiz = await _unitOfWork.Quizs.GetItem(u => u.Id == new Guid(request.QuizId));
			if (quiz == null) throw new CustomException("Quiz does not exist");
			var quizQuestions =  JsonSerializer.Deserialize<List<QuizQuestion>>(quiz.QuizQuestions);

			var quizQuestion =  quizQuestions[request.QuestionIndex];
			if (quizQuestion == null) throw new CustomException("Question does not exist");
			return ResponseModel.Send(quizQuestion);
		}

	}

	public class QuizQuestionResponse
	{
		public string? question { get; set; } = "";
		public string? mode { get; set; } = "";
		public List<Option>? options { get; set; }
		public string? optionsAnswer  = "";
		public string? trueFalseAnswer = "";
		public string? reason = "";
		public int? points { get; set; }
	}

	public class QuizQuestion
	{
		public string? question { get; set; } = "";
		public string? mode { get; set; } = "";
		public List<Option>? options { get; set; }
		public string? optionsAnswer { get; set; }= "";
		public string? trueFalseAnswer { get; set; }
		public string ?reason { get; set; } = "";
		public int? points { get; set; }
	}

	public class Option
	{
		public string Text { get; set; }
	}

	
}
