namespace TutorApplication.SharedModels.Requests
{
	public class UpdateStudentProfileInformationRequest
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Title { get; set; }
		public string About { get; set; }
	}
}
