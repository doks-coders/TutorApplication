using Microsoft.AspNetCore.Mvc;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;
using static TutorApplication.ApplicationCore.Services.CourseService;

namespace TutorApplication.Controllers
{
	public class PhotoController : BaseController
	{
		private readonly IPhotoService _photoService;
		private readonly IUnitOfWork _unitOfWork;

		public PhotoController(IPhotoService photoService, IUnitOfWork unitOfWork)
		{
			_photoService = photoService;
			_unitOfWork = unitOfWork;
		}

		[HttpPost("upload-photo/{id}")]
		public async Task<ActionResult> UploadImage(IFormFile file, string id=null)
		{
			return await _photoService.UploadImage(file);
		}

		[HttpPost("upload-user-photo/{userId}")]
		public async Task<ActionResult> UpdateUserImage(IFormFile file, Guid userId)
		{
			return await _photoService.UpdateUserImage(file, userId);
		}


		[HttpPost("upload-course-photo/{courseId}")]
		public async Task<ActionResult> UpdateCourseImage(IFormFile file,Guid courseId)
		{
			return await _photoService.UpdateCourseImage(file, courseId);
		}

		[HttpDelete("delete-photo/{photoId}")]
		public async Task<ActionResult> DeleteImage(Guid photoId)
		{
			return await _photoService.DeleteImage(photoId);
		}

		[HttpGet("set-names")]
		public async Task<ActionResult> SetFullNames()
		{
			var users = await _unitOfWork.Users.GetItems(u => u.Id != null);

			users.ToList().ForEach(val =>
			{
				val.FullName = val.LastName + " " + val.FirstName;
				val.FullNameBackwards = val.FirstName + " " + val.LastName;
			});
			await _unitOfWork.SaveChanges();
			return Ok();
		}


		[HttpGet("delete-user-groups")]
		public async Task<ActionResult> DeleteUserGroups()
		{
			var course_students = await  _unitOfWork.UserGroups.GetItems(u => u.Id != null);
			await _unitOfWork.UserGroups.DeleteItems(course_students);
			
			await _unitOfWork.SaveChanges();
			return Ok();
		}


		[HttpGet("delete-courses")]
		public async Task<ActionResult> DeleteCourses()
		{
			var courses = await _unitOfWork.Courses.GetItems(u => u.Id != null);
			courses.ToList().ForEach(val =>
			{
				val.isDetailsCompleted = false;
			});
			await _unitOfWork.SaveChanges();
			return Ok();
		}
	}
}
