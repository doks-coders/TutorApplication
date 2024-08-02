using Azure.Core;
using System.Drawing.Printing;
using System.Text.Json;
using TutorApplication.ApplicationCore.Extensions;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.ApplicationCore.Utils;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Requests;
using TutorApplication.SharedModels.Responses;
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
			var course = await _unitOfWork.Courses.GetItem(u => u.Id == courseGroupId);
			var messages = await _unitOfWork.Messages.GetItems(
				u => u.isCourseGroup == true
				&&
				u.CourseId == courseGroupId, includeProperties: "Sender,Photos,Quiz");

			JsonSerializerOptions options = new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
			var memos = JsonSerializer.Deserialize<IEnumerable<Memo>>(course.Memos, options);

			var responseMessages = messages.OrderBy(m => m.Created).Select(u => new MessageResponse()
			{
				SenderId = (Guid)u.SenderId,
				Content = u.Content,
				Created = u.Created,
				Id = u.Id,
				Photos = u.Photos.ConvertPhotoToPhotoResponse(),
				SenderName = u.Sender.LastName + " " + u.Sender.FirstName + (u.SenderId == course.TutorId ? " (Tutor)" : ""),
				Quiz=u.Quiz!=null?new QuizResponse() { Id=u.Quiz.Id,QuizName=u.Quiz.QuizName}:null
				
			}).ToList();

			var userGroup = await _unitOfWork.UserGroups.GetItem(u => u.UserId == senderId && u.CourseGroupId == courseGroupId && u.isGroup == "true");

			var missedMessages = responseMessages.Where(u => u.Created > userGroup.LastSeen);
			if (missedMessages.Any())
			{
				responseMessages.Insert(responseMessages.Count() - missedMessages.Count(), new MessageResponse()
				{
					SenderId = default,
					Content = "You have new messages",
					Created = default,
					Mode = "alert",
					Id = default,
					SenderName = default
				});
			}

			//Adding Memo Topics
			var finalResponse = new List<MessageResponse>();
			var bookInfos = memos.Select(u => u.BookInfo).ToList();


			var bookDates = memos.Select(u =>
			{
				var memoDate = TutorServiceUtils.ConvertDateStringToDate(u.Date);
				string[] time = u.Time.Split(":");
				var hour = int.Parse(time[0]);
				var minute = int.Parse(time[1]);
				var updatedDate = memoDate.AddHours(hour).AddMinutes(minute);
				return updatedDate;
			}).ToList();

			responseMessages.ForEach(el =>
			{
				var infoIndex = bookDates.FindIndex(u => u < el.Created);

				if (infoIndex > -1)
				{
					finalResponse.Add(
						new MessageResponse()
						{
							SenderId = default,
							Content = $"Topic - {bookInfos[infoIndex]}",
							Created = default,
							Mode = "alert",
							Id = default,
							SenderName = default
						}
					);
					bookDates.RemoveAt(infoIndex);
					bookInfos.RemoveAt(infoIndex);
				}
				
				finalResponse.Add(el);
				
			
			});




			return finalResponse;
		}

		public async Task<ResponseModel> CheckMessagingAllowed(Guid recieverId, Guid senderId)
		{
			var course_student = await _unitOfWork.CourseStudents.GetItem(u => (u.TutorId == recieverId && u.StudentId == senderId));
			if (course_student != null) return ResponseModel.Send(true);
			return ResponseModel.Send(false);
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
			, includeProperties: "Sender,Photos");




			var responseMessages = messages.OrderBy(m => m.Created).Select(u => new MessageResponse()
			{
				SenderId = (Guid)u.SenderId,
				Content = u.Content,
				Created = u.Created,
				Id = u.Id,
				SenderName = u.Sender.LastName + " " + u.Sender.FirstName,
				Photos = u.Photos.ConvertPhotoToPhotoResponse()
			}).ToList();

			var userGroups = await _unitOfWork.UserGroups.GetItems(u => u.UserId == senderId && u.RecieverId == recieverId && u.isGroup == "false");
			var groupies = userGroups.ToList();
			var userGroup = await _unitOfWork.UserGroups.GetItem(u => u.UserId == senderId && u.RecieverId == recieverId && u.isGroup == "false");

			var missedMessages = responseMessages.Where(u => u.Created > userGroup.LastSeen);
			if (missedMessages.Any())
			{
				responseMessages.Insert(responseMessages.Count() - missedMessages.Count(), new MessageResponse()
				{
					SenderId = default,
					Content = "You have new messages",
					Created = default,
					Mode = "alert",
					Id = default,
					SenderName = default
				});
			}



			return responseMessages;
		}

		public async Task<Message> SendCourseGroupMessage(CourseGroupMessageRequest request, Guid senderId)
		{
			var isAdmin = false;
			var course = await _unitOfWork.Courses.GetItem(u => u.Id == request.CourseGroupId);
			if (course.TutorId == senderId)
			{
				isAdmin = true;
			}
			if (!isAdmin && request.QuizId != null) throw new CustomException("Not an admin");
		
			var message = new Message()
			{
				SenderId = senderId,
				CourseId = request.CourseGroupId,
				Content = request.Content,
				isCourseGroup = true,
				QuizId = request.QuizId
			};
			await _unitOfWork.Messages.AddItem(message);
			await _unitOfWork.SaveChanges();

			if (request.Photos != null)
			{
				await UpdateMessagePhotos(request.Photos, message.Id);

			}

			var user = await _unitOfWork.Users.GetItem(u => u.Id == message.SenderId);
			var quiz = await _unitOfWork.Quizs.GetItem(u => u.Id == request.QuizId);
			message.Sender = user;
			message.Quiz = quiz;
			return message;
		}

		public async Task<Message> SendDirectMessage(DirectMessageRequest request, Guid senderId)
		{
			var message = new Message()
			{
				SenderId = senderId,
				RecieverId = request.RecieverId,
				Content = request.Content,
			};

			await _unitOfWork.Messages.AddItem(message);
			await _unitOfWork.SaveChanges();

			if (request.Photos != null)
			{
				await UpdateMessagePhotos(request.Photos, message.Id);
			}

			var user = await _unitOfWork.Users.GetItem(u => u.Id == message.SenderId);
			message.Sender = user;
			return message;
		}

		private async Task UpdateMessagePhotos(IEnumerable<PhotoResponse> IncomingPhotos, Guid messageId)
		{
			var photosIds = IncomingPhotos.Select(e => e.Id.ToString());
			string photosIdsString = string.Join(",", photosIds.ToArray());
			var photos = await _unitOfWork.Photos.GetItems(u => photosIdsString.Contains(u.Id.ToString()));
			photos.ToList().ForEach(val =>
			{
				val.MessageId = messageId;
			});

			await _unitOfWork.SaveChanges();
		}
		public async Task<ResponseModel> GetContactsForStudents(Guid studentId)
		{
			var courseStuds = await _unitOfWork.CourseStudents.GetItems(u => u.StudentId == studentId, includeProperties: "Tutor,Course");

			var registeredCoursesTutors = courseStuds.GroupBy(u => u.TutorId).Select(e => e.First()).Select(u => new DisplayMessageContact()
			{
				DisplayName = u.Tutor.FirstName + " " + u.Tutor.LastName,
				Email = u.Tutor.Email,
				SubText = u.Tutor.AccountType != "Student" ? "Tutor" : "Student",
				RecieverId = u.TutorId,
				CourseGroupId = u.CourseId,
				IsGroup = false,
				Id = u.Tutor.NavigationId,
				IsOnline = false,
				ImageUrl = u.Tutor.ImageUrl,
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
			var courseStuds = await _unitOfWork.CourseStudents.GetItems(u => u.TutorId == tutorId && u.Course.isDetailsCompleted == true, includeProperties: "Student,Course");

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

			var missedGroups = await _unitOfWork.MissedMessage.GetItems(u => u.RecieverId == studentId && u.isGroup == "true");
			var missedGroupsList = missedGroups.ToList();


			var missedTutors = await _unitOfWork.MissedMessage.GetItems(u => u.RecieverId == studentId && u.isGroup == "false");
			var missedTutorsList = missedTutors.ToList();


			var courseStuds = await _unitOfWork.CourseStudents.GetItems(u => u.StudentId == studentId, includeProperties: "Tutor,Course");

			var registeredCoursesTutors = courseStuds.GroupBy(u => u.TutorId).Select(e => e.First()).Select(u =>

			{
				var sub = missedTutorsList.Where(message => message.SenderId == u.TutorId).ToList();

				return new DisplayMessageContact()
				{
					DisplayName = u.Tutor.FirstName + " " + u.Tutor.LastName,
					Email = u.Tutor.Email,
					SubText = u.Tutor.AccountType, //!= "Student" ? "Tutor" : "Student",
					RecieverId = u.TutorId,
					MissedMessagesCount = sub.Count(),
					CourseGroupId = u.CourseId,
					IsGroup = false,
					Id = u.Tutor.NavigationId,
					IsOnline = false,
					ImageUrl = u.Tutor.ImageUrl,
					lastTimeAvailable = ""

				};
			});

			var registeredCourses = courseStuds.GroupBy(u => u.CourseId).Select(e => e.First()).Select(u =>
			{
				var sub = missedGroupsList.Where(message => message.CourseGroupId == u.CourseId).ToList();

				return new DisplayMessageContact()
				{
					DisplayName = u.Course.CourseTitle,
					SubText = "Course Group",
					RecieverId = u.TutorId,
					CourseGroupId = u.CourseId,
					IsGroup = true,
					Id = u.Course.NavigationId,
					IsOnline = false,
					ImageUrl = u.Course.ImageUrl,
					lastTimeAvailable = "",
					MissedMessagesCount = sub.Count()

				};
			});
			var displayContacts = registeredCourses.ToList().Concat(registeredCoursesTutors.ToList());
			return displayContacts.ToList();
		}


		public List<MissedMessage> MissedMessages = new();

		public async Task<List<DisplayMessageContact>> GetContactsForTutorswithHub(Guid tutorId)
		{
			//var user = await _unitOfWork.Users.GetItem(u => u.Id == tutorId, includeProperties: "IncomingMessages");

			//var courseMissedMessage = await _unitOfWork.Messages.GetItems(u => u.Created > user.LastSeen);

			var missedGroups = await _unitOfWork.MissedMessage.GetItems(u => u.RecieverId == tutorId && u.isGroup == "true");
			var missedGroupsList = missedGroups.ToList();


			var missedUsers = await _unitOfWork.MissedMessage.GetItems(u => u.RecieverId == tutorId && u.isGroup == "false");
			var missedUsersList = missedUsers.ToList();


			//var missedMessages = user.IncomingMessages.Where(u => u.Created > user.LastSeen).ToList();

			var courseStuds = await _unitOfWork.CourseStudents.GetItems(u => u.TutorId == tutorId && u.Course.isDetailsCompleted == true, includeProperties: "Student,Course");


			var registeredCoursesStudents = courseStuds.Where(u => u.StudentId != tutorId).GroupBy(u => u.StudentId).Select(e => e.First()).Select(u =>
			{
				var sub = missedUsersList.Where(message => message.SenderId == u.StudentId).ToList();


				return new DisplayMessageContact()
				{
					MissedMessagesCount = sub.Count(),
					DisplayName = u.Student.FirstName + " " + u.Student.LastName,
					Email = u.Student.Email,
					SubText = u.Student.AccountType,// != "Student" ? "Tutor" : "Student",
					RecieverId = u.StudentId,
					CourseGroupId = u.CourseId,
					IsGroup = false,
					Id = u.Student.NavigationId,
					IsOnline = false,
					ImageUrl = u.Student.ImageUrl,
					lastTimeAvailable = "",


				};
			}
			).ToList();

			var registeredCourses = courseStuds.GroupBy(u => u.CourseId).Select(e => e.First()).Select(u =>

			{
				var sub = missedGroupsList.Where(message => message.CourseGroupId == u.CourseId).ToList();


				return new DisplayMessageContact()
				{
					DisplayName = u.Course.CourseTitle,
					SubText = "Course Group",
					RecieverId = u.StudentId,
					CourseGroupId = u.CourseId,
					IsGroup = true,
					Id = u.Course.NavigationId,
					IsOnline = false,
					MissedMessagesCount = sub.Count(),
					ImageUrl = u.Course.ImageUrl,
					lastTimeAvailable = ""

				};
			}).ToList();

			var displayContacts = registeredCourses.ToList().Concat(registeredCoursesStudents.ToList());

			return displayContacts.ToList();

		}
	}
}
