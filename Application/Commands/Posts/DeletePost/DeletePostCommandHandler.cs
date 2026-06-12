using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.DeletePost
{
    public class DeletePostCommandHandler(
        IPostRepository postRepository,
        ICurrentUserService currentUserService) : IRequestHandler<DeletePostCommand, ErrorOr<Unit>>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<Unit>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);
            if (post is null)
                return Errors.Post.NotFound;

            if (!_currentUserService.IsOwnerOrAdmin(post.UserId))
                return Errors.Post.Unauthorized;

            await _postRepository.DeleteAsync(post, cancellationToken);
            return Unit.Value;
        }
    }
}
