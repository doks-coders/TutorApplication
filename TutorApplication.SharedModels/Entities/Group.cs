namespace TutorApplication.SharedModels.Entities
{
	public class Group
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public List<Connection> Connections { get; set; } = new();
	}
}
