using BiiGBackend.Models.SharedModels;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Responses;

namespace BiiGBackend.ApplicationCore.Services
{
	public class PhotoService : IPhotoService
	{
		private readonly Cloudinary cloudinary;
		private readonly IUnitOfWork _unitOfWork;
		public PhotoService(IOptions<CloudinarySettings> config, IUnitOfWork unitOfWork)
		{
			Account acc = new Account()
			{
				Cloud = config.Value.CloudName,
				ApiKey = config.Value.ApiKey,
				ApiSecret = config.Value.ApiSecret,
			};
			cloudinary = new Cloudinary(acc);
			_unitOfWork = unitOfWork;
		}
		public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
		{
			var uploadResult = new ImageUploadResult();
			if (file.Length > 0)
			{
				//create file stream
				using (var stream = file.OpenReadStream()) //we use "using" so it can be disposed earlier
				{
					ImageUploadParams uploadParams = new ImageUploadParams()
					{
						File = new FileDescription(file.Name, stream),
						Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
						Folder = "tutor-photo-backend"
					};

					uploadResult = await cloudinary.UploadAsync(uploadParams);
				};
			}
			return uploadResult;

		}

		public async Task<DeletionResult> DeletePhotoAsync(string publicId)
		{
			DeletionResult deletionResult = new DeletionResult();
			//publicId = "/" + publicId;
			DeletionParams deleteParams = new DeletionParams(publicId);


			var res = cloudinary.Destroy(deleteParams);
			return res;

		}

		
		public async Task<ResponseModel> DeleteImage(Guid Id)
		{
			var photo = await _unitOfWork.Photos.GetItem(u => u.Id == Id);


			DeletionResult res = await DeletePhotoAsync(photo.PublicId);

			if (res.Error != null) throw new CustomException(res.Error.Message);

			await _unitOfWork.Photos.DeleteItem(photo);
			await _unitOfWork.SaveChanges();

			return ResponseModel.Send(photo.Id);

		}


		public async Task<ResponseModel> UpdateCourseImage(IFormFile file, Guid courseId)
		{
			ImageUploadResult res = await AddPhotoAsync(file);

			if (res.Error != null) throw new CustomException(res.Error.Message);
			Photo photo = new Photo()
			{
				Url = res.SecureUrl.AbsoluteUri,
				PublicId = res.PublicId,
				CourseId = courseId
			};
			await _unitOfWork.Photos.AddItem(photo);
			await _unitOfWork.SaveChanges();
			var course = await _unitOfWork.Courses.GetItem(u => u.Id == courseId);
			course.PhotoId = photo.Id;
			await _unitOfWork.SaveChanges();
			return ResponseModel.Send(new PhotoResponse() { CourseId=(Guid)photo.CourseId,Id=photo.Id,PublicId=photo.PublicId,Url=photo.Url});
		}


		public async Task<ResponseModel> UpdateUserImage(IFormFile file, Guid userId)
		{
			ImageUploadResult res = await AddPhotoAsync(file);

			if (res.Error != null) throw new CustomException(res.Error.Message);
			Photo photo = new Photo()
			{
				Url = res.SecureUrl.AbsoluteUri,
				PublicId = res.PublicId,
			};
			Console.WriteLine("Updated User Image");
			await _unitOfWork.Photos.AddItem(photo);
			await _unitOfWork.SaveChanges();
			var course = await _unitOfWork.Users.GetItem(u => u.Id == userId);
			course.PhotoId = photo.Id;
			await _unitOfWork.SaveChanges();
			return ResponseModel.Send(new PhotoResponse() {Id = photo.Id, PublicId = photo.PublicId, Url = photo.Url });;

		}
	}
}
