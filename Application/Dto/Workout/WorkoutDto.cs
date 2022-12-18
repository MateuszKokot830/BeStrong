namespace Application.Dto
{
    public class WorkoutDto
    {
        public int? UserId { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public IEnumerable<WorkoutExerciseDto> WorkoutExercises { get; set; }
    }
}