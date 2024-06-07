using MediatR;

namespace TutorApplication.Infrastructure.Mediators.Tutor.GetTutor
{
	public class GetTutorHandler : IRequestHandler<GetTutorRequest, GetTutorResponse>
	{
		public Task<GetTutorResponse> Handle(GetTutorRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
