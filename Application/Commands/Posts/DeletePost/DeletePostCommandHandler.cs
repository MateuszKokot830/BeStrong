using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Commands.Posts.DeletePost
{
    public class DeletePostCommandHandler(IPostRepository postRepository) : IRequestHandler<DeletePostCommand, Unit>
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var userPosts = await _postRepository.GetAllUserPostsAsync(request.UserId);
            var postToDelete = userPosts.FirstOrDefault(x => x.Id == request.PostId);

            if (postToDelete != null)
                await _postRepository.DeleteAsync(postToDelete);

            return Unit.Value;
        }
    }
}