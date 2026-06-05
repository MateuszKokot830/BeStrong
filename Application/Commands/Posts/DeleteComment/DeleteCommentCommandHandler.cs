using Application.Interfaces;
using MediatR;
using ErrorOr;

namespace Application.Commands.Posts.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Unit>
    {
        private readonly IPostRepository _postRepository;

        public DeleteCommentCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

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