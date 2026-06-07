using Application.Dto.Comment;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.CreateComment
{
    public record CreateCommentCommand(CommentCreateDto CommentCreateDto) : IRequest<ErrorOr<CommentDto>>;
}
