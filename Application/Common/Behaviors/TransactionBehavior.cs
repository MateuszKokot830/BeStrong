using Application.Interfaces.Common;
using MediatR;
using System.Transactions;

namespace Application.Common.Behaviors
{
    public sealed class TransactionBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (IsNotCommand())
                return await next();

            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var response = await next();

            await _unitOfWork.CommitAsync(cancellationToken);

            transactionScope.Complete();

            return response;
        }

        private static bool IsNotCommand() =>
            !typeof(TRequest).Name.EndsWith("Command");
    }
}
