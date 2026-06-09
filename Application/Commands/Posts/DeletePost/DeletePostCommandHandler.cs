using Application.Interfaces.Repositories;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.DeletePost
{
    public class DeletePostCommandHandler(IPostRepository postRepository) : IRequestHandler<DeletePostCommand, ErrorOr<Unit>>
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<ErrorOr<Unit>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetPostByIdAsync(request.PostId, cancellationToken);

            if (post is null)
                return Errors.Post.NotFound;

            if (post.UserId != request.UserId)
                return Errors.Post.Unauthorized;

            await _postRepository.DeleteAsync(post, cancellationToken);

            return Unit.Value;
        }
    }
}
