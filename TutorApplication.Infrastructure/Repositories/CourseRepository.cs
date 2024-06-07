using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApplication.Infrastructure.Data;
using TutorApplication.Infrastructure.Repositories.Interfaces;
using TutorApplication.SharedModels.Entities;

namespace TutorApplication.Infrastructure.Repositories
{
	public class CourseRepository : BaseRepository<Course>, ICourseRepository
	{
		public CourseRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
