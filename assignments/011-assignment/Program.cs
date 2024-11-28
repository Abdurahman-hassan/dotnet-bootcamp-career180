using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using movieReservationSystem.Repositories;
using movieReservationSystem.Services;
using movieReservationSystem.Utils;
using Microsoft.OpenApi.Models;

namespace movieReservationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie Reservation API", Version = "v1" });
            });

            // Configure MongoDB
            var configuration = builder.Configuration;
            var mongoDbSettings = configuration.GetSection("MongoDb").Get<MongoDbSettings>();
            if (mongoDbSettings?.ConnectionString != null && mongoDbSettings.DatabaseName != null)
            {
                builder.Services.AddSingleton(new MongoDbContext(mongoDbSettings.ConnectionString, mongoDbSettings.DatabaseName));
            }
            else
            {
                throw new System.Exception("MongoDB settings are not configured correctly.");
            }

            // Register repositories
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Register services
            builder.Services.AddScoped<MovieService>();
            builder.Services.AddScoped<ReservationService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AuthService>();

            // this is CORS config for testing locally 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Reservation API V1");
                });
            }

            // app.UseHttpsRedirection(); disabled, since i test locally then no need for HTTPS redirection

            app.UseCors("AllowAllOrigins");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }

    public class MongoDbSettings
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}