using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetOneRepMax
{
    public record GetOneRepMaxQuery(int Weight, int Reps) : IRequest<ErrorOr<int>>;
}
