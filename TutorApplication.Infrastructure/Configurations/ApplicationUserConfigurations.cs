using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorApplication.SharedModels.Entities;

namespace TutorApplication.Infrastructure.Configurations
{
	public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.HasMany(u => u.UserRoles)
				.WithOne(u => u.AppUser)
				.HasForeignKey(u => u.UserId)
				.OnDelete(DeleteBehavior.NoAction);

			builder.HasMany(u => u.IncomingMessages)
				.WithOne(u => u.Reciever)
				.HasForeignKey(u => u.RecieverId)
				.OnDelete(DeleteBehavior.NoAction);

			builder.HasMany(u => u.OutgoingMessages)
				.WithOne(u => u.Sender)
				.HasForeignKey(u => u.SenderId)
				.OnDelete(DeleteBehavior.NoAction);

		}
	}
}
