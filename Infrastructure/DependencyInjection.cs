using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Repositories;
using Infrastructure.Searchers;
using Infrastructure.Searchers.Decorators;
using Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddDbContext<DataContext>((sp, options) =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
                options.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>());
                options.EnableSensitiveDataLogging();
            });

            // Repositories (write side)
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IWorkoutRepository, WorkoutRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Searchers (read side)
            services.AddScoped<IUserSearcher, UserSearcher>();
            services.AddScoped<IPostSearcher, PostSearcher>();
            services.AddScoped<IWorkoutSearcher, WorkoutSearcher>();
            services.AddScoped<IWorkoutPlanSearcher, WorkoutPlanSearcher>();
            services.AddScoped<ExerciseSearcher>();
            services.AddScoped<IExerciseSearcher>(sp =>
                new CachedExerciseSearcher(
                    sp.GetRequiredService<ExerciseSearcher>(),
                    sp.GetRequiredService<IMemoryCache>()));
            services.AddMemoryCache();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            return services;
        }
    }
}
