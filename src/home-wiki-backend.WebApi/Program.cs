using home_wiki_backend.DAL.Data;
using home_wiki_backend.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace home_wiki_backend
{
    /// <summary>
    /// The main entry point for the Home Wiki backend application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main method which configures and runs the web application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureDependencyInjection();

            // Register DbContext with Azure SQL connection string
            var connectionString = builder.Configuration
                .GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<DbWikiContext>(options =>
                options.UseSqlServer(connectionString));

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Home Wiki API",
                    Description = "API documentation for the Home Wiki backend"
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Auto-apply migrations
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DbWikiContext>();
                db.Database.Migrate();
            }

            // Enable CORS globally
            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Home Wiki API v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            // Auto-apply migrations
            app.ApplyMigrationsAndSeed();

            app.Run();
        }
    }
}
