using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Enums
{
	public enum ErrorCodes
	{
		IncorrectPassword,
		UserDoesNotExist,
		CourseDoesNotExist,
		UserExist,
		ErrorWhileSaving,
		ErrorWhileAdding


	}

	public enum ErrorIdentifiers
	{
		ValidationErrors,
		CustomError,
		UnclassifiedError,
	}
}
