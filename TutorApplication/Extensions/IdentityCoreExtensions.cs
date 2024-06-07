using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TutorApplication.Infrastructure.Data;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Models;

namespace TutorApplication.Extensions
{
	public static class IdentityCoreExtensions
	{
		public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services, IConfiguration config)
		{
			services.Configure<JwtOptions>(config.GetSection("JwtOptions"));

			services.AddIdentityCore<ApplicationUser>(e =>
			{
				e.Password.RequireNonAlphanumeric = false;
				e.Password.RequireLowercase = false;
				e.Password.RequireUppercase = false;
				e.Password.RequiredLength = 6;
			})
				.AddRoles<ApplicationRole>()
				.AddRoleManager<RoleManager<ApplicationRole>>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(e =>
			{
				e.TokenValidationParameters = new()
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JwtOptions:Key").Value))
				};

				e.Events = new JwtBearerEvents()
				{
					OnMessageReceived = (context) =>
					{
						var accessToken = context.Request.Query["access_token"];
						var path = context.HttpContext.Request.Path;

						if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
						{
							context.Token = accessToken;
						}

						return Task.CompletedTask;
					}
				};
			});
			return services;
		}
	}
}
