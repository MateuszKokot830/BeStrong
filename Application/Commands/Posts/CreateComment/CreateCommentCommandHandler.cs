using AutoMapper;
using MediatR;
using Domain.Entities;
using Domain.Errors;
using ErrorOr;
using Application.Dto.Comment;
using Application.Interfaces.Repositories;

namespace Application.Commands.Posts.CreateComment
{
    public class CreateCommentCommandHandler(IPostRepository postRepository, IMapper mapper) : IRequestHandler<CreateCommentCommand, ErrorOr<CommentDto>>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var comment = _mapper.Map<Comment>(request.CommentCreateDto);
                await _postRepository.CreateCommentAsync(comment, cancellationToken);
                return _mapper.Map<CommentDto>(comment);
            }
            catch (Exception)
            {
                return Errors.Comment.CreationFailed;
            }
        }
    }
}
