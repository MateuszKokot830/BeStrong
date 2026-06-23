using Application.Dto.Comment;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.UpdateComment
{
    public record UpdateCommentCommand(int CommentId, UpdateCommentDto UpdateCommentDto) : IRequest<ErrorOr<CommentDto>>;
}
