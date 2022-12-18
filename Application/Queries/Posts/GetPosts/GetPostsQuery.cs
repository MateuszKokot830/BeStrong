using Application.Dto;
using MediatR;

namespace Application.Queries.Posts.GetPosts
{
    public class GetPostsQuery : IRequest<IEnumerable<PostDto>>
    { 
    }
}