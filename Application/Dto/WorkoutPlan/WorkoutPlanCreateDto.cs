namespace Application.Dto
{
    public class WorkoutPlanCreateDto
    {
        public int CreatedById { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<WorkoutDto> Workouts { get; set; }
    }
}