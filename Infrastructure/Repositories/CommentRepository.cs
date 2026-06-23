using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CommentRepository(DataContext context) : ICommentRepository
    {
        private readonly DataContext _context = context;

        public async Task<Comment?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
            await _context.Comments
                .Include(c => c.Likes)
                .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

        public Task AddAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            _context.Comments.Add(comment);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            _context.Entry(comment).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            _context.Comments.Remove(comment);
            return Task.CompletedTask;
        }

        public Task AddLikeAsync(CommentLike like, CancellationToken cancellationToken = default)
        {
            _context.Set<CommentLike>().Add(like);
            return Task.CompletedTask;
        }

        public Task DeleteLikeAsync(CommentLike like, CancellationToken cancellationToken = default)
        {
            _context.Set<CommentLike>().Remove(like);
            return Task.CompletedTask;
        }
    }
}
