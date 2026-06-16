using Domain.Aggregates;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class WorkoutPlanRepository(DataContext context) : BaseRepository<WorkoutPlan>(context), IWorkoutPlanRepository
    {
        protected override IQueryable<WorkoutPlan> GetQueryable() => _context.WorkoutPlans
            .Include(w => w.Workouts).ThenInclude(w => w.WorkoutExercises)
            .Include(u => u.UsedBy);

        public async Task<WorkoutPlan?> GetUserCurrentWorkoutPlanAsync(int id, CancellationToken cancellationToken = default)
        {
            return await GetQueryable()
                .SingleOrDefaultAsync(w => w.Id == id, cancellationToken);
        }
    }
}
