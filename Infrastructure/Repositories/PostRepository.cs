using Domain.Aggregates;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(DataContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Post>> GetAllUserPostsAsync(int id)
        {
            return await _context.Posts.Include(c => c.Comments).Where(c => c.UserId == id).ToListAsync();
        }

        public async Task<IReadOnlyList<Post>> GetAllFollowedUsersPostsAsync(List<int> ids)
        {
            return await _context.Posts.Include(c => c.Comments).Where(c => ids.Contains(c.UserId)).ToListAsync();
        }

        public async Task CreateCommentAsync(Comment comment)
        {
           _context.Set<Comment>().Add(comment);
           await _context.SaveChangesAsync();
        }
    }
}