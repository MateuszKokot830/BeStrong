using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Application.Mappings;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {   
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSingleton(AutoMapperConfig.Initialize());

            return services;
        }
    }
}