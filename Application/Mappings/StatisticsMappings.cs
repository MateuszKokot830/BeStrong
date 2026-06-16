using Application.Dto.Statistics;
using Domain.ValueObjects;

namespace Application.Mappings
{
    public static class StatisticsMappings
    {
        public static StatisticsDto ToDto(this Statistics statistics) => new(
            statistics.TotalWorkouts,
            statistics.TotalExercises,
            statistics.TotalSets,
            statistics.AvgWorkoutsPerWeek,
            statistics.AvgExercisesPerWorkout,
            statistics.AvgSetsPerWorkout,
            statistics.FavouriteExercise
        );
    }
}
