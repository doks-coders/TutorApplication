using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorApplication.SharedModels.Entities;

namespace TutorApplication.Infrastructure.Configurations
{
	public class GroupConfigurations : IEntityTypeConfiguration<Group>
	{
		public void Configure(EntityTypeBuilder<Group> builder)
		{
			builder.HasMany(u => u.Connections)
				.WithOne(u => u.Group)
				.HasForeignKey(u => u.GroupId)
				.OnDelete(DeleteBehavior.NoAction);
		}
	}
}
