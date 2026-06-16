using Application.Dto.Post;
using Application.Interfaces.Repositories;
using Application.Mappings;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.CreatePost
{
    public class CreatePostCommandHandler(IPostRepository postRepository)
        : IRequestHandler<CreatePostCommand, ErrorOr<PostDto>>
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<ErrorOr<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = request.PostCreateDto.ToEntity();
            await _postRepository.AddAsync(post, cancellationToken);
            return post.ToDto();
        }
    }
}
