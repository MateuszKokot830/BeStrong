using Application.Dto.WorkoutPlan;
using Application.Helpers;
using Application.Helpers.Criteria;
using Application.Interfaces.Searchers;
using Application.Mappings;
using Domain.Aggregates;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Searchers
{
    public sealed class WorkoutPlanSearcher(DataContext context) : IWorkoutPlanSearcher
    {
        private readonly DataContext _context = context;

        private IQueryable<WorkoutPlan> GetQueryable() =>
            _context.WorkoutPlans
                .AsNoTracking()
                .Include(p => p.WorkoutTemplates).ThenInclude(t => t.Exercises).ThenInclude(e => e.Exercise)
                .Include(p => p.UsedBy)
                .Include(p => p.CreatedBy);

        public async Task<WorkoutPlanDto?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var plan = await GetQueryable().SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
            return plan?.ToDto();
        }

        public async Task<PaginationList<WorkoutPlanDto>> GetPagedAsync(WorkoutPlanSearchCriteria criteria, int requestingUserId, IReadOnlyList<int>? followedUserIds, CancellationToken cancellationToken = default)
        {
            var query = GetQueryable()
                .Where(p =>
                    (criteria.CreatedBy == CreatedByFilter.OnlyMyself && p.CreatedById == requestingUserId) ||
                    (criteria.CreatedBy == CreatedByFilter.OnlyFollowers && p.IsPublic && followedUserIds != null && followedUserIds.Contains(p.CreatedById)) ||
                    (criteria.CreatedBy == CreatedByFilter.All && (p.IsPublic || p.CreatedById == requestingUserId)))
                .Where(p => criteria.Category == null || p.Category == criteria.Category)
                .Where(p => criteria.Name == null || (p.Name != null && EF.Functions.Like(p.Name, $"%{criteria.Name}%")))
                .Where(p => criteria.OwnerName == null || (p.CreatedBy != null &&
                    EF.Functions.Like((p.CreatedBy.Name ?? "") + " " + (p.CreatedBy.Surname ?? ""), $"%{criteria.OwnerName}%")))
                .OrderBy(p => p.Name);

            var count = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((criteria.PageNumber - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginationList<WorkoutPlanDto>(items.Select(p => p.ToDto()).ToList(), count, criteria.PageNumber, criteria.PageSize);
        }
    }
}
