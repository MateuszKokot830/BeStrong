using AutoMapper;
using MediatR;
using Domain.Aggregates;
using Application.Interfaces.Repositories;

namespace Application.Commands.Posts.CreatePost
{
    public class CreatePostCommandHandler(IPostRepository postRepository, IMapper mapper) : IRequestHandler<CreatePostCommand>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = _mapper.Map<Post>(request.PostCreateDto);
            await _postRepository.AddAsync(post);

            return Unit.Value;
        }
    }
}