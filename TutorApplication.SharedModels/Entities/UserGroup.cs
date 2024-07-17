namespace TutorApplication.SharedModels.Entities
{
	public class UserGroup
	{
		public int Id { get; set; }
		public Guid? UserId { get; set; }
		public Guid? CourseGroupId { get; set; }
		public Guid? RecieverId { get; set; }
		public string? isGroup { get; set; }
		public DateTime? LastSeen { get; set; } = DateTime.UtcNow;
		public string UserName { get; set; }
		public string GroupName { get; set; }
	}
}
