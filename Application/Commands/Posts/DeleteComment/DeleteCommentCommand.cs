using MediatR;

namespace Application.Commands.Posts.DeleteComment
{
    public record DeleteCommentCommand(int CommentId, int UserId) : IRequest<Unit>;
}