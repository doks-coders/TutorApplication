using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.Infrastructure.Mediators.Tutor.DeleteCourse
{
	public class DeleteCourseHandler : IRequestHandler<DeleteCourseRequest, DeleteCourseResponse>
	{
		public Task<DeleteCourseResponse> Handle(DeleteCourseRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
