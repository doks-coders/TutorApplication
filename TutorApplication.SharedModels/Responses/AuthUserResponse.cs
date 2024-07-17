using TutorApplication.SharedModels.Entities;

namespace TutorApplication.SharedModels.Responses
{
	public class AuthUserResponse
	{
		public string UserName { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;
	}

	public class UnfinishedUserResponse
	{
		public string? ImageUrl { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? AuthStep { get; set; }
		public string? Interests { get; set; }
		public string Title { get; set; }
		public string About { get; set; }
		public PhotoResponse Photo { get; set; }
	}
}
