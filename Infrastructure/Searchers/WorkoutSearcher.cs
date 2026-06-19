using Application.Dto.Workout;
using Application.Interfaces.Searchers;
using Application.Mappings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Searchers
{
    public sealed class WorkoutSearcher(DataContext context) : IWorkoutSearcher
    {
        private readonly DataContext _context = context;

        public async Task<IReadOnlyList<WorkoutDto>> FindByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            var workouts = await _context.Workouts
                .AsNoTracking()
                .Include(w => w.WorkoutExercises)
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.Date)
                .ToListAsync(cancellationToken);

            return workouts.Select(w => w.ToDto()).ToList();
        }
    }
}
