using Microsoft.EntityFrameworkCore;
using TutorApplication.ApplicationCore.SignalR.Hubs;
using TutorApplication.ApplicationCore.SignalR.Persistence;
using TutorApplication.Extensions;
using TutorApplication.Infrastructure.Data;
using TutorApplication.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<VideoGroupChats>();
builder.Services.AddSingleton<OnlineUsers>();
builder.Services.ConfigureAppServices(builder.Configuration);
builder.Services.ConfigureIdentityServices(builder.Configuration);

var connString = "";
if (builder.Environment.IsDevelopment())
{
	connString = builder.Configuration.GetConnectionString("DefaultConnection");
}
else
{

	// Use connection string provided at runtime by FlyIO.
	var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

	// Parse connection URL to connection string for Npgsql
	connUrl = connUrl.Replace("postgres://", string.Empty);
	var pgUserPass = connUrl.Split("@")[0];
	var pgHostPortDb = connUrl.Split("@")[1];
	var pgHostPort = pgHostPortDb.Split("/")[0];
	var pgDb = pgHostPortDb.Split("/")[1];
	var pgUser = pgUserPass.Split(":")[0];
	var pgPass = pgUserPass.Split(":")[1];
	var pgHost = pgHostPort.Split(":")[0];
	var pgPort = pgHostPort.Split(":")[1];
	var updatedHost = pgHost.Replace("flycast", "internal");

	connString = $"Server={updatedHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";

}

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
	opt.UseNpgsql(connString);
});
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin",
		builder => builder
			.WithOrigins("https://localhost:4200")
			.AllowAnyMethod()
			.AllowAnyHeader()
			.AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors(u => u.AllowAnyHeader().AllowAnyMethod()
.AllowCredentials()
.WithOrigins("https://localhost:4200", "http://localhost:4200"));


app.UseMiddleware<ExceptionMiddleware>();
app.MapHub<MessageHub>("hubs/message");
app.MapHub<VideoCallHub>("hubs/video");
app.MapHub<PresenceHub>("hubs/presence");
app.MapFallbackToController("Index", "Fallback");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;

	try
	{
	
		var db = services.GetRequiredService<ApplicationDbContext>();
		//await db.Database.EnsureDeletedAsync();
		await db.Database.MigrateAsync();
		var logger = services.GetService<ILogger<Program>>();
		logger.LogInformation("Migration Successfull");

	}
	catch (Exception ex)
	{
		var logger = services.GetService<ILogger<Program>>();
		logger.LogError(ex, "An Error Occurred during Migration");
	}

}
app.Run();
