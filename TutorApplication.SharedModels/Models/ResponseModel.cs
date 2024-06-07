using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TutorApplication.SharedModels.Models
{
	
	public class ResponseModel : ActionResult
	{
		public object Data { get; set; }
		public HttpStatusCode StatusCode { get; set; }

		public override async Task ExecuteResultAsync(ActionContext context)
		{
			context.HttpContext.Response.StatusCode = (int)StatusCode;
			context.HttpContext.Response.ContentType = "application/json";
			JsonSerializerOptions options = new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};
			await context.HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(Data, options)));
		}

		public static ResponseModel Send(object? Data = null, HttpStatusCode? StatusCode = HttpStatusCode.OK)
		{
			var response = new ResponseModel();
			response.Data = Data ?? string.Empty;
			response.StatusCode = (HttpStatusCode)StatusCode;
			return response;
		}
	}
}
