using Application.Dto.Post;
using Application.Interfaces.Searchers;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUserPostsByUsername
{
    public class GetUserPostsByUsernameQueryHandler(
        IUserSearcher userSearcher,
        IPostSearcher postSearcher) : IRequestHandler<GetUserPostsByUsernameQuery, ErrorOr<IEnumerable<PostDto>>>
    {
        private readonly IUserSearcher _userSearcher = userSearcher;
        private readonly IPostSearcher _postSearcher = postSearcher;

        public async Task<ErrorOr<IEnumerable<PostDto>>> Handle(GetUserPostsByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userSearcher.FindByUsernameAsync(request.Username, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            var posts = await _postSearcher.FindByUserIdAsync(user.Id, cancellationToken);
            return posts.ToList();
        }
    }
}
