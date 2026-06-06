using Domain.Aggregates;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class PostRepository(DataContext context) : BaseRepository<Post>(context), IPostRepository
    {
        public override async Task AddAsync(Post post)
        {
            post.CreatedDate = DateTime.Now;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }
        public async Task<IReadOnlyList<Post>> GetAllUserPostsAsync(int userId)
        {
            return await _context.Posts.Include(c => c.Comments)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Post>> GetAllFollowedUsersPostsAsync(List<int> userIds)
        {
            return await _context.Posts.Include(c => c.Comments)
                .OrderByDescending(c => c.CreatedDate)
                .Where(c => userIds.Contains(c.UserId))
                .ToListAsync();
        }

        public async Task CreateCommentAsync(Comment comment)
        {
            comment.CreatedDate = DateTime.Now;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}