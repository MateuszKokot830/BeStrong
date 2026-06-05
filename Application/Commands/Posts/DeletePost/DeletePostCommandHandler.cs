using Application.Interfaces;
using MediatR;
using ErrorOr;

namespace Application.Commands.Posts.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Unit>
    {
        private readonly IPostRepository _postRepository;

        public DeletePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

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