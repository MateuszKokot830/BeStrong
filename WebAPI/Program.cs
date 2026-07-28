using Application;
using Domain.Aggregates;
using DomainEntities = Domain.Entities;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebAPI.Extensions;
using WebAPI.Middleware;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

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

    app.UseSerilogRequestLogging();
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
    var userManager = services.GetRequiredService<UserManager<User>>();
    await SeedData.SeedUserData(userManager);
    await SeedData.SeedExerciseData(context);
    await SeedData.SeedRolesAsync(
        services.GetRequiredService<RoleManager<DomainEntities.Role>>(),
        userManager);

    Log.Information("Starting BeStrong API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "BeStrong API terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
