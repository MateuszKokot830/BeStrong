using Application;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using WebAPI.Extensions;
using WebAPI.Middleware;

var logger = LogManager.Setup().LoadConfigurationFromFile("NLog.config").GetCurrentClassLogger();
try
{
    logger.Debug("Program has been started");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseNLog();

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddRouting(options => options.LowercaseUrls = true);
    builder.Services.AddControllers();
    builder.Services.AddCors();
    builder.Services.AddIdentityServices(builder.Configuration);
    builder.Services.AddSwaggerGen(c => c.EnableAnnotations());

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
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

    app.MapControllers();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await SeedData.SeedUserData(context);
    await SeedData.SeedExerciseData(context);

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Program has stopped due to exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
