using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.UnlikeComment
{
    public record UnlikeCommentCommand(int CommentId) : IRequest<ErrorOr<Unit>>;
}
