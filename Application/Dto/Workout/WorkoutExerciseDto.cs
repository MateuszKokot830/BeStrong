namespace Application.Dto.Workout
{
    public class WorkoutExerciseDto
    {
        public int Sets { get; set; }
        public int Reps { get; set; }
        public decimal? Weight { get; set; }
        public int ExerciseId { get; set; }
        public int WorkoutId { get; set; }
    }
}