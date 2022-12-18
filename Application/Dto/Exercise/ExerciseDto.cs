namespace Application.Dto
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; }
        public string Description { get; set; }
        public TimeSpan? Duration { get; set; }
        public int? Sets { get; set; }
        public int? Reps { get; set; }
    }
}