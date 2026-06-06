namespace Application.Dto.Statistics
{
    public class StatisticsDto
    {
        public int TotalWorkouts { get; set; }
        public int TotalExercises { get; set; }
        public int TotalSets { get; set; }
        public decimal AvgWorkoutsPerWeek { get; set; }
        public decimal AvgExercisesPerWorkout { get; set; }
        public decimal AvgSetsPerWorkout { get; set; }
        public string? FavouriteExercise { get; set; }
    }
}