using Application.Dto.Statistics;
using MediatR;

namespace Application.Queries.Workouts.GetWorkoutStatistics
{
    public record GetWorkoutStatisticsQuery(int UserId) : IRequest<StatisticsDto>;
}