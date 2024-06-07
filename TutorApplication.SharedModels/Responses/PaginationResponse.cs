using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Responses
{
	public class PaginationResponse
	{
		public int TotalItems{ get; set; }
		public int TotalPages { get; set; }
		public int PageNumber { get; set; }
		public object Items { get; set; }
	}
}
