using Domain.Services;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetOneRepMax
{
    public sealed class GetOneRepMaxQueryHandler : IRequestHandler<GetOneRepMaxQuery, ErrorOr<int>>
    {
        public Task<ErrorOr<int>> Handle(GetOneRepMaxQuery request, CancellationToken cancellationToken)
        {
            var result = OneRepMaxCalculator.Calculate(request.Weight, request.Reps);
            return Task.FromResult((ErrorOr<int>)result);
        }
    }
}
