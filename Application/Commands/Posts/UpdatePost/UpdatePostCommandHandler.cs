using Application.Dto.Post;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Common;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.UpdatePost
{
    public class UpdatePostCommandHandler(
        IPostRepository postRepository,
        ICurrentUserService currentUserService) : IRequestHandler<UpdatePostCommand, ErrorOr<PostDto>>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<PostDto>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);
            if (post is null)
                return Errors.Post.NotFound;

            if (!_currentUserService.IsOwnerOrAdmin(post.UserId))
                return Errors.Post.Unauthorized;

            if (post.Type == PostType.Normal && request.UpdatePostDto.Description is null)
                return Errors.Post.DescriptionRequired;

            post.Description = request.UpdatePostDto.Description;
            post.UpdatedDate = DateTime.UtcNow;

            await _postRepository.UpdateAsync(post, cancellationToken);
            return post.ToDto();
        }
    }
}
