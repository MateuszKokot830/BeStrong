using Application.Interfaces.Repositories;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.DeleteComment
{
    public class DeleteCommentCommandHandler(IPostRepository postRepository) : IRequestHandler<DeleteCommentCommand, ErrorOr<Unit>>
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<ErrorOr<Unit>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var userPosts = await _postRepository.GetAllUserPostsAsync(request.UserId, cancellationToken);
            var commentToDelete = userPosts.SelectMany(x => x.Comments)
                .FirstOrDefault(x => x.Id == request.CommentId);

            if (commentToDelete is null)
                return Errors.Post.NotFound;

            await _postRepository.DeleteCommentAsync(commentToDelete, cancellationToken);

            return Unit.Value;
        }
    }
}