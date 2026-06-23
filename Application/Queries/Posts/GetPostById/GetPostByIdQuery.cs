using Application.Dto.Post;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetPostById
{
    public record GetPostByIdQuery(int PostId) : IRequest<ErrorOr<PostDto>>;
}
