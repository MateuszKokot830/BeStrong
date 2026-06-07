using Application.Dto.Statistics;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetWorkoutStatistics
{
    public record GetWorkoutStatisticsQuery(int UserId) : IRequest<ErrorOr<StatisticsDto>>;
}
