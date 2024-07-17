using BiiGBackend.ApplicationCore.Services;
using BiiGBackend.Models.SharedModels;
using TutorApplication.ApplicationCore.Services;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.ApplicationCore.SignalR.Services;
using TutorApplication.Infrastructure.Data;
using TutorApplication.Infrastructure.Repositories;
using TutorApplication.Infrastructure.Repositories.Interfaces;

namespace TutorApplication.Extensions
{
	public static class ApplicationCoreExtensions
	{
		public static IServiceCollection ConfigureAppServices(this IServiceCollection services, IConfiguration config)
		{

			services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(typeof(ApplicationDbContext).Assembly));

			services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<ITutorService, TutorService>();
			services.AddScoped<IStudentService, StudentService>();
			services.AddScoped<ICourseService, CourseService>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IMessageService, MessageService>();
			services.AddScoped<IMessageHubServices, MessageHubServices>();
			services.AddScoped<IHubServices, HubServices>();
			services.AddScoped<IPhotoService, PhotoService>();
			return services;
		}
	}
}
