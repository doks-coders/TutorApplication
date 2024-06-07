using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
