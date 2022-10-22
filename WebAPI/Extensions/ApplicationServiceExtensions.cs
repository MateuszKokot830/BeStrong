using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace WebAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton(AutoMapperConfig.Initialize());

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAppUserService, AppUserService>();

            services.AddScoped<IAppUserRepository, AppUserRepository>();

            services.AddDbContext<DataContext>();

            return services;
        }
    }
}