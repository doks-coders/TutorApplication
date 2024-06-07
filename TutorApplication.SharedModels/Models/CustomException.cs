using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using TutorApplication.SharedModels.Enums;

namespace TutorApplication.SharedModels.Models
{
	public class CustomException : Exception
	{
		public CustomException(IEnumerable<ValidationFailure> exceptions) : base($"{ErrorIdentifiers.ValidationErrors}:{string.Join(", ", exceptions.Select(e => e.ErrorMessage).ToArray())}")
		{

		}

		public CustomException(IEnumerable<IdentityError> exceptions) : base($"{ErrorIdentifiers.ValidationErrors}:{string.Join(", ", exceptions.Select(e => e.Description).ToArray())}")
		{

		}

		public CustomException(ErrorCodes error) : base($"{ErrorIdentifiers.CustomError}:{error}")
		{

		}

		public CustomException(string error) : base($"{ErrorIdentifiers.UnclassifiedError}:{error}")
		{

		}



	}
}
