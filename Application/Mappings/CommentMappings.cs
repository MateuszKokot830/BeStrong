using Application.Dto.Comment;
using Domain.Entities;

namespace Application.Mappings
{
    public static class CommentMappings
    {
        public static CommentDto ToDto(this Comment comment) => new(
            comment.Id,
            comment.UserId,
            comment.Description,
            comment.CreatedDate,
            comment.Likes,
            comment.PostId
        );

        public static Comment ToEntity(this CommentCreateDto dto) => new()
        {
            UserId = dto.UserId,
            Description = dto.Description,
            CreatedDate = DateTime.UtcNow,
            PostId = dto.PostId
        };
    }
}
