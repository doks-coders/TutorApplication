using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Responses;

namespace TutorApplication.ApplicationCore.Extensions
{
	public static class PhotoExtensions
	{
		public static PhotoResponse ConvertPhotoToPhotoResponse(this Photo photo)
		{
			return new PhotoResponse()
			{
				CourseId = photo.CourseId,
				Id = photo.Id,
				PublicId = photo.PublicId,
				Url = photo.Url
			};
		}
	}
}
