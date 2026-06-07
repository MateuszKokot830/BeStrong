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
            var userPosts = await _postRepository.GetAllUserPostsAsync(request.UserId, cancellationToken);
            var postToDelete = userPosts.FirstOrDefault(x => x.Id == request.PostId);

            if (postToDelete is null)
                return Errors.Post.NotFound;

            await _postRepository.DeleteAsync(postToDelete, cancellationToken);

            return Unit.Value;
        }
    }
}