using Domain.Aggregates;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories
{
    public class WorkoutRepository  : BaseRepository<Workout>, IWorkoutRepository
    {
        public WorkoutRepository(DataContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Workout>> GetUserWorkoutsAsync(int id)
        {
            return await _context.Workouts.Include(w => w.WorkoutExercises).Where(w => w.UserId == id).ToListAsync();
        }
    }
}