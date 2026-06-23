using Application.Dto.Post;
using Application.Interfaces.Searchers;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetPostById
{
    public class GetPostByIdQueryHandler(IPostSearcher postSearcher)
        : IRequestHandler<GetPostByIdQuery, ErrorOr<PostDto>>
    {
        private readonly IPostSearcher _postSearcher = postSearcher;

        public async Task<ErrorOr<PostDto>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postSearcher.FindByIdAsync(request.PostId, cancellationToken);
            if (post is null)
                return Errors.Post.NotFound;

            return post;
        }
    }
}
