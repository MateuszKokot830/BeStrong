namespace Application.Dto.Workout
{
    public class WorkoutDto
    {
        public int? UserId { get; set; }
        public DateTime Date { get; set; }
        public string? Name { get; set; }
        public ICollection<WorkoutExerciseDto> WorkoutExercises { get; set; } = [];
    }
}