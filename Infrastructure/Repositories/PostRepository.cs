using Domain.Aggregates;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class PostRepository(DataContext context) : BaseRepository<Post>(context), IPostRepository
    {
        public override async Task AddAsync(Post post, CancellationToken cancellationToken = default)
        {
            post.CreatedDate = DateTime.Now;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Post>> GetAllUserPostsAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _context.Posts.Include(c => c.Comments)
                .Where(c => c.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Post>> GetAllFollowedUsersPostsAsync(List<int> userIds, CancellationToken cancellationToken = default)
        {
            return await _context.Posts.Include(c => c.Comments)
                .OrderByDescending(c => c.CreatedDate)
                .Where(c => userIds.Contains(c.UserId))
                .ToListAsync(cancellationToken);
        }

        public async Task CreateCommentAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            comment.CreatedDate = DateTime.Now;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCommentAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
