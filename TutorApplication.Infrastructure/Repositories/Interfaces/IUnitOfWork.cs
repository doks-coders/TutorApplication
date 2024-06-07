﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.Infrastructure.Repositories.Interfaces
{
	public interface IUnitOfWork
	{
		IApplicationUserRepository Users { get; }
		ICourseRepository Courses { get; }
		public ICourseStudentRepository CourseStudents { get; }
		public IMessageRepository Messages { get; }
		public IConnectionRepository Connections { get; }
		public IGroupRepository Groups { get; }
		Task<bool> SaveChanges();
	}
}
