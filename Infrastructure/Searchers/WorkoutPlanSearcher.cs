using Application.Dto.WorkoutPlan;
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
                .Include(p => p.WorkoutTemplates).ThenInclude(t => t.Exercises)
                .Include(p => p.UsedBy);

        public async Task<WorkoutPlanDto?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var plan = await GetQueryable().SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
            return plan?.ToDto();
        }

        public async Task<IReadOnlyList<WorkoutPlanDto>> GetPublicAsync(CancellationToken cancellationToken = default)
        {
            var plans = await GetQueryable()
                .Where(p => p.IsPublic)
                .ToListAsync(cancellationToken);

            return plans.Select(p => p.ToDto()).ToList();
        }
    }
}
