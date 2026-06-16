using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetOneRepMax
{
    public sealed class GetOneRepMaxQueryHandler : IRequestHandler<GetOneRepMaxQuery, ErrorOr<int>>
    {
        public Task<ErrorOr<int>> Handle(GetOneRepMaxQuery request, CancellationToken cancellationToken)
        {
            var result = (int)Math.Ceiling(request.Weight / (1.0278 - 0.0278 * request.Reps));
            return Task.FromResult((ErrorOr<int>)result);
        }
    }
}
