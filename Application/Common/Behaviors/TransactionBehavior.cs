using Application.Interfaces.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Common.Behaviors
{
    public sealed class TransactionBehavior<TRequest, TResponse>(
        IUnitOfWork unitOfWork,
        ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (IsNotCommand())
                return await next();

            var requestName = typeof(TRequest).Name;
            var sw = Stopwatch.StartNew();

            _logger.LogInformation("Beginning transaction for {RequestName}", requestName);

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var response = await next();

                await _unitOfWork.CommitAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                sw.Stop();
                _logger.LogInformation("Committed transaction for {RequestName} in {ElapsedMs}ms", requestName, sw.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogWarning(ex, "Rolling back transaction for {RequestName} after {ElapsedMs}ms", requestName, sw.ElapsedMilliseconds);
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        private static bool IsNotCommand() =>
            !typeof(TRequest).Name.EndsWith("Command");
    }
}
