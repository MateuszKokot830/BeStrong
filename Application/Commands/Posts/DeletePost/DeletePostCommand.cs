using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.DeletePost
{
    public class DeletePostCommand : IRequest <Unit>
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}