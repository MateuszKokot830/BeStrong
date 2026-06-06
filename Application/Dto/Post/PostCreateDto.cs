namespace Application.Dto.Post
{
    public class PostCreateDto
    {
        public int UserId { get; set; }
        public string? Description { get; set; }
        public int? WorkoutId { get; set; }
        public int? WorkoutPlan { get; set; }
    }
}