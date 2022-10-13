using Infrastructure.Data;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Add services to the container.
services.AddControllers();
services.AddDbContext<DataContext>();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
       .UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
