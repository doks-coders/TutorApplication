using MediatR;

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
