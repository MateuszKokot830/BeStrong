using Application.Dto;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.CreateComment
{
    public class CreateCommentCommand : IRequest
    {
        public CommentCreateDto CommentCreateDto { get; set; }
    }
}