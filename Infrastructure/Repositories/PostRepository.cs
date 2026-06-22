using Application.Interfaces.Repositories;
using Domain.Aggregates;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PostRepository(DataContext context) : BaseRepository<Post>(context), IPostRepository
    {
        protected override IQueryable<Post> GetQueryable() => _context.Posts.Include(p => p.Likes);

        public Task AddLikeAsync(PostLike like, CancellationToken cancellationToken = default)
        {
            _context.Set<PostLike>().Add(like);
            return Task.CompletedTask;
        }

        public Task DeleteLikeAsync(PostLike like, CancellationToken cancellationToken = default)
        {
            _context.Set<PostLike>().Remove(like);
            return Task.CompletedTask;
        }
    }
}
