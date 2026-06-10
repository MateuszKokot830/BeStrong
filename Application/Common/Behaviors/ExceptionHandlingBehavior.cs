using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors
{
    public sealed class ExceptionHandlingBehavior<TRequest, TResponse>(
        ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception for request {RequestName}", typeof(TRequest).Name);

                var error = Error.Unexpected(
                    code: "UnhandledException",
                    description: "An unexpected error occurred. Please try again later.");

                return (TResponse)(object)new List<Error> { error };
            }
        }
    }
}
