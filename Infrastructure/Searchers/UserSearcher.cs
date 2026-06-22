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

        public async Task<UserDto?> FindByUsernameAsync(string username, CancellationToken cancellationToken = default) =>
            await _context.Users
                .AsNoTracking()
                .Where(u => u.NormalizedUserName == username.ToUpperInvariant())
                .Select(UserMappings.Selector)
                .SingleOrDefaultAsync(cancellationToken);

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
                .Where(u => criteria.Username == null || u.UserName != criteria.Username)
                .OrderBy(u => u.UserName)
                .Select(UserMappings.Selector);

            return await PaginationList<UserDto>.CreateAsync(query, criteria.PageNumber, criteria.PageSize, cancellationToken);
        }

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
