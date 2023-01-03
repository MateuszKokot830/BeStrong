using Domain.Aggregates;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class WorkoutPlanRepository : BaseRepository<WorkoutPlan>, IWorkoutPlanRepository
    {
        public WorkoutPlanRepository(DataContext context) : base(context)
        {
        }

        public override async Task AddAsync(WorkoutPlan workoutPlan)
        {
            if (workoutPlan.Workouts != null && workoutPlan.Workouts.Any())
            {
                foreach(var plan in workoutPlan.Workouts)
                {
                    if (plan.WorkoutExercises != null && plan.WorkoutExercises.Any())
                    {
                        _context.WorkoutExercises.AddRange(plan.WorkoutExercises);
                        _context.Workouts.Add(plan);
                    }
                }
                _context.WorkoutPlans.Add(workoutPlan);
            }

            await _context.SaveChangesAsync();
        }

        public override async Task<IReadOnlyList<WorkoutPlan>> GetAllAsync()
        {
            return await _context.WorkoutPlans
                .Include(w => w.Workouts).ThenInclude(w => w.WorkoutExercises)
                .Include(u => u.UsedBy)
                .ToListAsync();
        }

        public async Task<WorkoutPlan> GetUserCurrentWorkoutPlanAsync(int id)
        {
            return await _context.WorkoutPlans
                .Include(w => w.Workouts).ThenInclude(w => w.WorkoutExercises)
                .Include(u => u.UsedBy)
                .Where(w => w.Id == id)
                .SingleOrDefaultAsync();
        }

    }
}