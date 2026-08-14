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
                .Include(p => p.UsedBy);

        public async Task<WorkoutPlanDto?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var plan = await GetQueryable().SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
            return plan?.ToDto();
        }

        public async Task<PaginationList<WorkoutPlanDto>> GetPagedAsync(WorkoutPlanSearchCriteria criteria, int requestingUserId, CancellationToken cancellationToken = default)
        {
            var query = GetQueryable()
                .Where(p => criteria.OnlyOwn ? p.CreatedById == requestingUserId : (p.IsPublic || p.CreatedById == requestingUserId))
                .Where(p => criteria.Category == null || p.Category == criteria.Category)
                .Where(p => criteria.Name == null || (p.Name != null && EF.Functions.Like(p.Name, $"%{criteria.Name}%")))
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
