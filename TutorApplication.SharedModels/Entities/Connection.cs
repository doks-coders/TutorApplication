namespace TutorApplication.SharedModels.Entities
{
	public class Connection
	{
		public Guid Id { get; set; }
		public Guid GroupId { get; set; }
		public Group Group { get; set; }
		public string ConnectionURL { get; set; }
		public string Username { get; set; }
		public string GroupName { get; set; }
	}
}
