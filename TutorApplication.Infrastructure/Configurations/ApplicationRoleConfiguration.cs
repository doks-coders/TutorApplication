using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorApplication.SharedModels.Entities;

namespace TutorApplication.Infrastructure.Configurations
{
	public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
	{
		public void Configure(EntityTypeBuilder<ApplicationRole> builder)
		{
			builder.HasMany(u => u.UserRoles)
				.WithOne(u => u.AppRole)
				.HasForeignKey(u => u.RoleId)
				.OnDelete(DeleteBehavior.NoAction);
		}
	}
}
