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
