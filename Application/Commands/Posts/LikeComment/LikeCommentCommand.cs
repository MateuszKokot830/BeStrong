using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.LikeComment
{
    public record LikeCommentCommand(int CommentId) : IRequest<ErrorOr<Unit>>;
}
