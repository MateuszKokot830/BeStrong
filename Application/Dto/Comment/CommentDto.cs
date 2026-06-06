namespace Application.Dto.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Likes { get; set; }
        public int PostId { get; set; }
    }
}