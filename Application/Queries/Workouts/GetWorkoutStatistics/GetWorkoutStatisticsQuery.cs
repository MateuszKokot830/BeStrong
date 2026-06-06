using Application.Dto.Statistics;
using MediatR;

namespace Application.Queries.Workouts.GetWorkoutStatistics
{
    public class GetWorkoutStatisticsQuery : IRequest<StatisticsDto>
    {
        public int UserId { get; set; }
    }
}