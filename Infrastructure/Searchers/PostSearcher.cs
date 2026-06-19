using Application.Dto.Post;
using Application.Interfaces.Searchers;
using Application.Mappings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Searchers
{
    public sealed class PostSearcher(DataContext context) : IPostSearcher
    {
        private readonly DataContext _context = context;

        public async Task<IReadOnlyList<PostDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var posts = await _context.Posts
                .AsNoTracking()
                .Include(p => p.Comments)
                .OrderBy(p => p.CreatedDate)
                .ToListAsync(cancellationToken);

            return posts.Select(p => p.ToDto()).ToList();
        }

        public async Task<IReadOnlyList<PostDto>> FindByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            var posts = await _context.Posts
                .AsNoTracking()
                .Include(p => p.Comments)
                .Where(p => p.UserId == userId)
                .OrderBy(p => p.CreatedDate)
                .ToListAsync(cancellationToken);

            return posts.Select(p => p.ToDto()).ToList();
        }

        public async Task<IReadOnlyList<PostDto>> FindByUserIdsAsync(IReadOnlyCollection<int> userIds, CancellationToken cancellationToken = default)
        {
            var posts = await _context.Posts
                .AsNoTracking()
                .Include(p => p.Comments)
                .Where(p => userIds.Contains(p.UserId))
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync(cancellationToken);

            return posts.Select(p => p.ToDto()).ToList();
        }
    }
}
