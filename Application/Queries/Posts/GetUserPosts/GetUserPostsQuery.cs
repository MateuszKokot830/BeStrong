using Application.Dto;
using MediatR;

namespace Application.Queries.Posts.GetUserPosts
{
    public class GetUserPostsQuery : IRequest<IEnumerable<PostDto>>
    {
        public int UserId { get; set; }
    }
}