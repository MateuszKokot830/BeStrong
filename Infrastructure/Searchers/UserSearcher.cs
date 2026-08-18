using Application.Dto.User;
using Application.Helpers;
using Application.Helpers.Criteria;
using Application.Interfaces.Searchers;
using Application.Mappings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Searchers
{
    public sealed class UserSearcher(DataContext context) : IUserSearcher
    {
        private readonly DataContext _context = context;

        public async Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.UserName)
                .Select(UserMappings.Selector)
                .ToListAsync(cancellationToken);

        public async Task<UserDto?> FindByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.NormalizedUserName == username.ToUpperInvariant())
                .Select(UserMappings.Selector)
                .SingleOrDefaultAsync(cancellationToken);

            return user?.WithComputedWorkoutSince();
        }

        public async Task<UserDto?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == id)
                .Select(UserMappings.Selector)
                .SingleOrDefaultAsync(cancellationToken);

            return user?.WithComputedWorkoutSince();
        }

        public async Task<IReadOnlyList<UserDto>> FindByIdsAsync(IReadOnlyCollection<int> ids, CancellationToken cancellationToken = default) =>
            await _context.Users
                .AsNoTracking()
                .Where(u => ids.Contains(u.Id))
                .Select(UserMappings.Selector)
                .ToListAsync(cancellationToken);

        public async Task<PaginationList<UserDto>> GetPagedAsync(UserSearchCriteria criteria, CancellationToken cancellationToken = default)
        {
            var query = _context.Users
                .AsNoTracking()
                .Where(u => criteria.ExcludeUsername == null || u.UserName != criteria.ExcludeUsername)
                .Where(u => criteria.Username == null || (u.UserName != null && EF.Functions.Like(u.UserName, $"%{criteria.Username}%")))
                .Where(u => criteria.Gender == null || u.Gender == criteria.Gender)
                .Where(u => criteria.Country == null || (u.Country != null && EF.Functions.Like(u.Country, $"%{criteria.Country}%")))
                .Where(u => criteria.City == null || (u.City != null && EF.Functions.Like(u.City, $"%{criteria.City}%")))
                .OrderBy(u => u.UserName)
                .Select(UserMappings.Selector);

            return await PaginationList<UserDto>.CreateAsync(query, criteria.PageNumber, criteria.PageSize, cancellationToken);
        }

        public async Task<IReadOnlyList<int>> GetFollowedUserIdsAsync(int userId, CancellationToken cancellationToken = default) =>
            await _context.Followers
                .AsNoTracking()
                .Where(f => f.UserId == userId)
                .Select(f => f.FollowedUserId)
                .ToListAsync(cancellationToken);

        public async Task<bool> ExistsAsync(int userId, CancellationToken cancellationToken = default) =>
            await _context.Users.AsNoTracking().AnyAsync(u => u.Id == userId, cancellationToken);

        public async Task<DateTime?> GetWorkoutStartDateAsync(int userId, CancellationToken cancellationToken = default) =>
            await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.DateOfWorkoutStart)
                .SingleOrDefaultAsync(cancellationToken);
    }
}
