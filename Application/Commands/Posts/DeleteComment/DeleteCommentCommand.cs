using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.DeleteComment
{
    public record DeleteCommentCommand(int CommentId) : IRequest<ErrorOr<Unit>>;
}
