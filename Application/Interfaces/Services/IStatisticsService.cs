using Application.Dto.Exercise;
using Application.Dto.Statistics;
using Application.Dto.Workout;

namespace Application.Interfaces.Services
{
    public interface IStatisticsService
    {
        StatisticsDto Calculate(
            IReadOnlyList<WorkoutDto> workouts,
            DateTime workoutStartDate,
            IReadOnlyList<ExerciseDto> exercises);
    }
}
