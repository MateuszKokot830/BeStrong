using Application.Dto.Comment;
using MediatR;

namespace Application.Commands.Posts.CreateComment
{
    public record CreateCommentCommand(CommentCreateDto CommentCreateDto) : IRequest;
}