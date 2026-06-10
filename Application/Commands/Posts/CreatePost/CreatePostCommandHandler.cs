using Application.Dto.Post;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Aggregates;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.CreatePost
{
    public class CreatePostCommandHandler(IPostRepository postRepository, IMapper mapper)
        : IRequestHandler<CreatePostCommand, ErrorOr<PostDto>>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = _mapper.Map<Post>(request.PostCreateDto);
            await _postRepository.AddAsync(post, cancellationToken);
            return _mapper.Map<PostDto>(post);
        }
    }
}
