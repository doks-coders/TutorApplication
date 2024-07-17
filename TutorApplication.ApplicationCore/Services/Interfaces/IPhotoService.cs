using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApplication.SharedModels.Models;

namespace TutorApplication.ApplicationCore.Services.Interfaces
{
	
	public interface IPhotoService
	{
		Task<DeletionResult> DeletePhotoAsync(string publicId);
		Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
		Task<ResponseModel> UpdateCourseImage(IFormFile file, Guid courseId);
		Task<ResponseModel> UpdateUserImage(IFormFile file, Guid userId);
		Task<ResponseModel> DeleteImage(Guid Id);
	}
}
