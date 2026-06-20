namespace Application.Dto.Comment
{
    public record CommentCreateDto(
        string? Description,
        int PostId
    );
}
