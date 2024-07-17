using Microsoft.AspNetCore.Mvc;
using TutorApplication.ApplicationCore.Services.Interfaces;
using static TutorApplication.ApplicationCore.Services.CourseService;

namespace TutorApplication.Controllers
{
	public class PhotoController : BaseController
	{
		private readonly IPhotoService _photoService;

		public PhotoController(IPhotoService photoService)
		{
			_photoService = photoService;
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
	}
}
