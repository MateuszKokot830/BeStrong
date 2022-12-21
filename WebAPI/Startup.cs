using Infrastructure.Data;
using Application.Interfaces;
using Application.Mappings;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Extensions;
using WebAPI.Middleware;
using Infrastructure;
using Application;
using Microsoft.EntityFrameworkCore;
using Infrastructure.SeedData;
using NLog;

namespace WebAPI
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers();
            services.AddCors();
            services.AddIdentityServices(_config);
            services.AddSwaggerGen(c => 
            {
                c.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync();
            await SeedData.SeedUserData(context);
            await SeedData.SeedExerciseData(context);
        }
    }
}
