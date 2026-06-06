namespace Application.Dto.Comment
{
    public class CommentCreateDto
    {
        public int UserId { get; set; }
        public string? Description { get; set; }
        public int PostId { get; set; }
    }
}