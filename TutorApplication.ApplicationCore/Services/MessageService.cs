using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses.Messages;

namespace TutorApplication.ApplicationCore.Services
{
	public class MessageService : IMessageService
	{
		private readonly IUnitOfWork _unitOfWork;

		public MessageService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<ResponseModel> DeleteMessage(Guid messageId)
		{
			var message = await _unitOfWork.Messages.GetItem(u => u.Id == messageId);
			message.isDeleted = true;
			await _unitOfWork.SaveChanges();
			return ResponseModel.Send("Deleted");
		}
		/// <summary>
		/// Get course group message
		/// </summary>
		/// <param name="courseGroupId">This is the course group Id</param>
		/// <param name="senderId">This is the sender Id</param>
		/// <returns></returns>
		public async Task<List<MessageResponse>> GetCourseGroupMessage(Guid courseGroupId, Guid senderId)
		{
			var messages = await _unitOfWork.Messages.GetItems(
				u => u.isCourseGroup == true
				&&
				u.CourseId == courseGroupId);

			var responseMessages = messages.OrderBy(m => m.Created).Select(u => new MessageResponse()
			{
				SenderId = (Guid)u.SenderId,
				Content = u.Content,
				Created = u.Created,
				Id = u.Id
			}).ToList();
			return responseMessages;
		}

		public async Task<List<MessageResponse>> GetDirectMessages(Guid recieverId, Guid senderId)
		{
			var messages = await _unitOfWork.Messages.GetItems(
				u =>
			u.RecieverId == recieverId
			&&
			u.SenderId == senderId
			||
			u.RecieverId == senderId
			&&
			u.SenderId == recieverId
			);
			var responseMessages = messages.OrderBy(m => m.Created).Select(u => new MessageResponse()
			{
				SenderId = (Guid)u.SenderId,
				Content = u.Content,
				Created = u.Created,
				Id = u.Id
			}).ToList();

			return responseMessages;
		}

		public async Task<Message> SendCourseGroupMessage(CourseGroupMessageRequest request, Guid senderId)
		{
			var message = new Message()
			{
				SenderId = senderId,
				CourseId = request.CourseGroupId,
				Content = request.Content,
				isCourseGroup = true
			};
			await _unitOfWork.Messages.AddItem(message);

			await _unitOfWork.SaveChanges();

			return message;
		}

		public async Task<Message> SendDirectMessage(DirectMessageRequest request, Guid senderId)
		{
			var message = new Message()
			{
				SenderId = senderId,
				RecieverId = request.RecieverId,
				Content = request.Content
			};
			await _unitOfWork.Messages.AddItem(message);

			await _unitOfWork.SaveChanges();

			return message;
		}

		public async Task<ResponseModel> GetContactsForStudents(Guid studentId)
		{
			var courseStuds = await _unitOfWork.CourseStudents.GetItems(u => u.StudentId == studentId, includeProperties: "Tutor,Course");

			var registeredCoursesTutors = courseStuds.GroupBy(u => u.TutorId).Select(e => e.First()).Select(u => new DisplayMessageContact()
			{
				DisplayName = u.Tutor.FirstName + " " + u.Tutor.LastName,
				Email = u.Tutor.Email,
				SubText = "Tutor",
				RecieverId = u.TutorId,
				CourseGroupId = u.CourseId,
				IsGroup = false,
				Id = u.Tutor.NavigationId,
				IsOnline = false,
				ImageUrl = "",
				lastTimeAvailable = ""

			});

			var registeredCourses = courseStuds.GroupBy(u => u.CourseId).Select(e => e.First()).Select(u => new DisplayMessageContact()
			{
				DisplayName = u.Course.CourseTitle,
				SubText = "Course Group",
				RecieverId = u.TutorId,
				CourseGroupId = u.CourseId,
				IsGroup = true,
				Id = u.Course.NavigationId,
				IsOnline = false,
				ImageUrl = "",
				lastTimeAvailable = ""

			});
			var displayContacts = registeredCourses.ToList().Concat(registeredCoursesTutors.ToList());
			return ResponseModel.Send(displayContacts.ToList());
		}

		public async Task<ResponseModel> GetContactsForTutors(Guid tutorId)
		{
			var courseStuds = await _unitOfWork.CourseStudents.GetItems(u => u.TutorId == tutorId, includeProperties: "Student,Course");

			var registeredCoursesStudents = courseStuds.GroupBy(u => u.StudentId).Select(e => e.First()).Select(u => new DisplayMessageContact()
			{
				DisplayName = u.Student.FirstName + " " + u.Student.LastName,
				Email = u.Student.Email,
				SubText = "Student",
				RecieverId = u.StudentId,
				CourseGroupId = u.CourseId,
				IsGroup = false,
				Id = u.Student.NavigationId,
				IsOnline = false,
				ImageUrl = "",
				lastTimeAvailable = "",


			});

			var registeredCourses = courseStuds.GroupBy(u => u.CourseId).Select(e => e.First()).Select(u => new DisplayMessageContact()
			{
				DisplayName = u.Course.CourseTitle,
				SubText = "Course Group",
				RecieverId = u.StudentId,
				CourseGroupId = u.CourseId,
				IsGroup = true,
				Id = u.Course.NavigationId,
				IsOnline = false,
				ImageUrl = "",
				lastTimeAvailable = ""

			});
			var displayContacts = registeredCourses.ToList().Concat(registeredCoursesStudents.ToList());

			return ResponseModel.Send(displayContacts.ToList());
		}

		public async Task<List<DisplayMessageContact>> GetContactsForStudentsWithHub(Guid studentId)
		{
			var courseStuds = await _unitOfWork.CourseStudents.GetItems(u => u.StudentId == studentId, includeProperties: "Tutor,Course");

			var registeredCoursesTutors = courseStuds.GroupBy(u => u.TutorId).Select(e => e.First()).Select(u => new DisplayMessageContact()
			{
				DisplayName = u.Tutor.FirstName + " " + u.Tutor.LastName,
				Email = u.Tutor.Email,
				SubText = "Tutor",
				RecieverId = u.TutorId,
				CourseGroupId = u.CourseId,
				IsGroup = false,
				Id = u.Tutor.NavigationId,
				IsOnline = false,
				ImageUrl = "",
				lastTimeAvailable = ""

			});

			var registeredCourses = courseStuds.GroupBy(u => u.CourseId).Select(e => e.First()).Select(u => new DisplayMessageContact()
			{
				DisplayName = u.Course.CourseTitle,
				SubText = "Course Group",
				RecieverId = u.TutorId,
				CourseGroupId = u.CourseId,
				IsGroup = true,
				Id = u.Course.NavigationId,
				IsOnline = false,
				ImageUrl = "",
				lastTimeAvailable = ""

			});
			var displayContacts = registeredCourses.ToList().Concat(registeredCoursesTutors.ToList());
			return displayContacts.ToList();
		}

		public async Task<List<DisplayMessageContact>> GetContactsForTutorswithHub(Guid tutorId)
		{
			var courseStuds = await _unitOfWork.CourseStudents.GetItems(u => u.TutorId == tutorId, includeProperties: "Student,Course");

			var registeredCoursesStudents = courseStuds.GroupBy(u => u.StudentId).Select(e => e.First()).Select(u => new DisplayMessageContact()
			{
				DisplayName = u.Student.FirstName + " " + u.Student.LastName,
				Email = u.Student.Email,
				SubText = "Student",
				RecieverId = u.StudentId,
				CourseGroupId = u.CourseId,
				IsGroup = false,
				Id = u.Student.NavigationId,
				IsOnline = false,
				ImageUrl = "",
				lastTimeAvailable = "",


			});

			var registeredCourses = courseStuds.GroupBy(u => u.CourseId).Select(e => e.First()).Select(u => new DisplayMessageContact()
			{
				DisplayName = u.Course.CourseTitle,
				SubText = "Course Group",
				RecieverId = u.StudentId,
				CourseGroupId = u.CourseId,
				IsGroup = true,
				Id = u.Course.NavigationId,
				IsOnline = false,
				ImageUrl = "",
				lastTimeAvailable = ""

			});
			var displayContacts = registeredCourses.ToList().Concat(registeredCoursesStudents.ToList());

			return displayContacts.ToList();

		}
	}
}
