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

        public async Task CreateCommentAsync(Comment comment)
        {
           _context.Set<Comment>().Add(comment);
           await _context.SaveChangesAsync();
        }
    }
}