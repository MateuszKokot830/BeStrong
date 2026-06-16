using Domain.Aggregates;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class PostRepository(DataContext context) : BaseRepository<Post>(context), IPostRepository
    {
        protected override IQueryable<Post> GetQueryable() =>
            _context.Posts.Include(p => p.Comments);

        public override Task AddAsync(Post post, CancellationToken cancellationToken = default)
        {
            _context.Posts.Add(post);
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<Post>> GetAllUserPostsAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await GetQueryable()
                .Where(p => p.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Post>> GetAllFollowedUsersPostsAsync(List<int> userIds, CancellationToken cancellationToken = default)
        {
            return await GetQueryable()
                .Where(p => userIds.Contains(p.UserId))
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<Comment?> GetCommentByIdAsync(int commentId, CancellationToken cancellationToken = default)
        {
            return await _context.Comments
                .SingleOrDefaultAsync(c => c.Id == commentId, cancellationToken);
        }

        public Task CreateCommentAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            _context.Comments.Add(comment);
            return Task.CompletedTask;
        }

        public Task DeleteCommentAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            _context.Comments.Remove(comment);
            return Task.CompletedTask;
        }
    }
}
