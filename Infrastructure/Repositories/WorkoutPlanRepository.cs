using Domain.Aggregates;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class WorkoutPlanRepository(DataContext context) : BaseRepository<WorkoutPlan>(context), IWorkoutPlanRepository
    {
        public override async Task AddAsync(WorkoutPlan workoutPlan, CancellationToken cancellationToken = default)
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

            await _context.SaveChangesAsync(cancellationToken);
        }

        public override async Task<IReadOnlyList<WorkoutPlan>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.WorkoutPlans
                .Include(w => w.Workouts).ThenInclude(w => w.WorkoutExercises)
                .Include(u => u.UsedBy)
                .ToListAsync(cancellationToken);
        }

        public async Task<WorkoutPlan?> GetUserCurrentWorkoutPlanAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.WorkoutPlans
                .Include(w => w.Workouts).ThenInclude(w => w.WorkoutExercises)
                .Include(u => u.UsedBy)
                .Where(w => w.Id == id)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
