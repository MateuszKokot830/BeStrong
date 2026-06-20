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

    }
}
