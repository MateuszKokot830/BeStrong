using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.DeleteComment
{
    public class DeleteCommentCommand : IRequest <Unit>
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
    }
}