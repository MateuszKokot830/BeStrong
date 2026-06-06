using AutoMapper;
using MediatR;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Application.Commands.Posts.CreateComment
{
    public class CreateCommentCommandHandler(IPostRepository postRepository, IMapper mapper) : IRequestHandler<CreateCommentCommand>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = _mapper.Map<Comment>(request.CommentCreateDto);
            await _postRepository.CreateCommentAsync(comment);

            return Unit.Value;
        }
    }
}