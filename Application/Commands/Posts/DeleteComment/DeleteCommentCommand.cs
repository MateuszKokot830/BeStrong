using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.DeleteComment
{
    public record DeleteCommentCommand(int CommentId, int UserId) : IRequest<ErrorOr<Unit>>;
}