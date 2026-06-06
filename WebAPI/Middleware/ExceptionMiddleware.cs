using System.Net;
using System.Text.Json;
using WebAPI.Exceptions;
using Application.Interfaces.Services;

namespace WebAPI.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
    {
        private readonly RequestDelegate _next = next;
        private readonly IHostEnvironment _env = env;

        public async Task InvokeAsync(HttpContext context, INLoggerService logger)
        {
            try 
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment() 
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) 
                    : new ApiException(context.Response.StatusCode, "Internal Server Error");

                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}