using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
