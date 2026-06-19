using Application.Dto.Comment;
using Application.Interfaces.Repositories;
using Application.Mappings;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.CreateComment
{
    public class CreateCommentCommandHandler(ICommentRepository commentRepository)
        : IRequestHandler<CreateCommentCommand, ErrorOr<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository = commentRepository;

        public async Task<ErrorOr<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = request.CommentCreateDto.ToEntity();
            await _commentRepository.AddAsync(comment, cancellationToken);
            return comment.ToDto();
        }
    }
}
