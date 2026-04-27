using Microsoft.EntityFrameworkCore;
using System;
using TASKe.Data;
using TASKe.Models;
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<MyAppContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")));

// Register custom services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TaskService>();

// Add services to the container.
builder.Services.AddControllers();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// Run migrations + seed inside a try/catch so the app doesn't crash if the DB isn't ready
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        var db = scope.ServiceProvider.GetRequiredService<MyAppContext>();

        // Ensure database is created / migrated
        db.Database.Migrate();

        // Seed admin if not exists
        if (!db.Users.Any(u => u.Role == "Admin"))
        {
            db.Users.Add(new User
            {
                Email = "admin@test.com",
                PasswordHash = "admin123",
                Role = "Admin"
            });

            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        // Log the error and continue so the API can start for debugging
        logger.LogError(ex, "Database migration/seed failed. The application will continue to run so you can debug the issue.");
    }
}

// Configure the HTTP request pipeline.

// app.UseHttpsRedirection();

// Use CORS before Authorization
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
