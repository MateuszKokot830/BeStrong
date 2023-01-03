namespace Application.Dto
{
    public class WorkoutPlanDto
    {
        public int CreatedById { get; set; }
        public ICollection<UserDto> UsedBy { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<WorkoutDto> Workouts { get; set; }
    }
}