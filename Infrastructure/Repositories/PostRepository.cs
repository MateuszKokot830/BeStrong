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

        public override async Task AddAsync(Post post) 
        {
            post.CreatedDate = DateTime.Now;
           _context.Posts.Add(post);
           await _context.SaveChangesAsync();
        }
        public async Task<IReadOnlyList<Post>> GetAllUserPostsAsync(int id)
        {
            return await _context.Posts.Include(c => c.Comments).Where(c => c.UserId == id).ToListAsync();
        }

        public async Task<IReadOnlyList<Post>> GetAllFollowedUsersPostsAsync(List<int> ids)
        {
            return await _context.Posts.Include(c => c.Comments)
                .OrderByDescending(c => c.CreatedDate)
                .Where(c => ids.Contains(c.UserId))
                .ToListAsync();
        }

        public async Task CreateCommentAsync(Comment comment)
        {
            comment.CreatedDate = DateTime.Now;
           _context.Comments.Add(comment);
           await _context.SaveChangesAsync();
        }
    }
}