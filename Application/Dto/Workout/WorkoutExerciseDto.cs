namespace Application.Dto
{
    public class WorkoutExerciseDto
    {
        public int Sets { get; set; }
        public int Reps { get; set; }
        public Decimal? Weight { get; set; }
        public int ExerciseId { get; set; }
        public int WorkoutId { get; set; }
    }
}