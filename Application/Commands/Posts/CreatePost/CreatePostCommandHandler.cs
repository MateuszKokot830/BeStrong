using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Domain.Aggregates;

namespace Application.Commands.Posts.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public CreatePostCommandHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = _mapper.Map<Post>(request.PostCreateDto);
            await _postRepository.AddAsync(post);

            return Unit.Value;
        }
    }
}