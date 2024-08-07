﻿namespace TutorApplication.SharedModels.Entities
{
	public class BaseEntity
	{
		public Guid Id { get; set; }
		public Guid NavigationId { get; set; } = Guid.NewGuid();
		public DateTime Created { get; set; } = DateTime.UtcNow;
		public DateTime Updated { get; set; } = DateTime.UtcNow;

		public BaseEntity()
		{
			Updated = DateTime.UtcNow;
		}
	}
}
