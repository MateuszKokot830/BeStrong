using Application.Dto.Comment;
using MediatR;

namespace Application.Commands.Posts.CreateComment
{
    public class CreateCommentCommand : IRequest
    {
        public required CommentCreateDto CommentCreateDto { get; set; }
    }
}