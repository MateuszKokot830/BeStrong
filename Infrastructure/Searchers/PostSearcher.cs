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

        private IQueryable<Domain.Aggregates.Post> GetQueryable() =>
            _context.Posts
                .AsNoTracking()
                .Include(p => p.Likes)
                .Include(p => p.Comments).ThenInclude(c => c.Likes)
                .Include(p => p.Workout).ThenInclude(w => w!.WorkoutExercises).ThenInclude(we => we.Sets);

        public async Task<PostDto?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var post = await GetQueryable()
                .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

            return post?.ToDto();
        }

        public async Task<IReadOnlyList<PostDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var posts = await GetQueryable()
                .OrderBy(p => p.CreatedDate)
                .ToListAsync(cancellationToken);

            return posts.Select(p => p.ToDto()).ToList();
        }

        public async Task<IReadOnlyList<PostDto>> FindByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            var posts = await GetQueryable()
                .Where(p => p.UserId == userId)
                .OrderBy(p => p.CreatedDate)
                .ToListAsync(cancellationToken);

            return posts.Select(p => p.ToDto()).ToList();
        }

        public async Task<IReadOnlyList<PostDto>> FindByUserIdsAsync(IReadOnlyCollection<int> userIds, CancellationToken cancellationToken = default)
        {
            var posts = await GetQueryable()
                .Where(p => userIds.Contains(p.UserId))
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync(cancellationToken);

            return posts.Select(p => p.ToDto()).ToList();
        }
    }
}
