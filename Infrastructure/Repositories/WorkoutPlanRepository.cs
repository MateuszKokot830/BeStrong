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

        public override Task AddAsync(WorkoutPlan workoutPlan, CancellationToken cancellationToken = default)
        {
            if (workoutPlan.Workouts != null && workoutPlan.Workouts.Any())
            {
                foreach (var plan in workoutPlan.Workouts)
                {
                    if (plan.WorkoutExercises != null && plan.WorkoutExercises.Any())
                    {
                        _context.WorkoutExercises.AddRange(plan.WorkoutExercises);
                        _context.Workouts.Add(plan);
                    }
                }
                _context.WorkoutPlans.Add(workoutPlan);
            }

            return Task.CompletedTask;
        }

        public async Task<WorkoutPlan?> GetUserCurrentWorkoutPlanAsync(int id, CancellationToken cancellationToken = default)
        {
            return await GetQueryable()
                .SingleOrDefaultAsync(w => w.Id == id, cancellationToken);
        }
    }
}
