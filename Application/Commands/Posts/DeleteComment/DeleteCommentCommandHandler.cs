using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Commands.Posts.DeleteComment
{
    public class DeleteCommentCommandHandler(IPostRepository postRepository) : IRequestHandler<DeleteCommentCommand, Unit>
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var userPosts = await _postRepository.GetAllUserPostsAsync(request.UserId);
            var commentToDelete = userPosts.SelectMany(x => x.Comments)
                .FirstOrDefault(x => x.Id == request.CommentId);

            if (commentToDelete != null)
                await _postRepository.DeleteCommentAsync(commentToDelete);

            return Unit.Value;
        }
    }
}