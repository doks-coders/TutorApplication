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
	public class CourseConfiguration : IEntityTypeConfiguration<Course>
	{
		public void Configure(EntityTypeBuilder<Course> builder)
		{
			builder.HasMany(u=>u.Students).WithOne(u=> u.Course).HasForeignKey(u=>u.CourseId).OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(u => u.Photo).WithOne(u => u.Course).OnDelete(DeleteBehavior.NoAction);

		}
	}
}
