using Application.Dto.Comment;
using Application.Interfaces.Repositories;
using Application.Mappings;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.CreateComment
{
    public class CreateCommentCommandHandler(IPostRepository postRepository)
        : IRequestHandler<CreateCommentCommand, ErrorOr<CommentDto>>
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<ErrorOr<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = request.CommentCreateDto.ToEntity();
            await _postRepository.CreateCommentAsync(comment, cancellationToken);
            return comment.ToDto();
        }
    }
}
