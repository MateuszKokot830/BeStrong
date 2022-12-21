namespace Application.Dto
{
    public class PostDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? WorkoutId { get; set; }
        public int? WorkoutPlanId { get; set; }
        public int Likes { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
    }
}