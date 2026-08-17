using Application.Dto.Workout;
using Application.Helpers;
using Application.Helpers.Criteria;
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
                .ThenInclude(we => we.Sets)
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.Date)
                .ToListAsync(cancellationToken);

            return workouts.Select(w => w.ToDto()).ToList();
        }

        public async Task<PaginationList<WorkoutDto>> GetPagedAsync(WorkoutSearchCriteria criteria, int userId, CancellationToken cancellationToken = default)
        {
            var query = _context.Workouts
                .AsNoTracking()
                .Include(w => w.WorkoutExercises).ThenInclude(we => we.Sets)
                .Where(w => w.UserId == userId)
                .Where(w => criteria.DateFrom == null || w.Date >= criteria.DateFrom)
                .Where(w => criteria.DateTo == null || w.Date <= criteria.DateTo)
                .Where(w => criteria.Name == null || (w.Name != null && EF.Functions.Like(w.Name, $"%{criteria.Name}%")))
                .Where(w => criteria.ExerciseId == null || w.WorkoutExercises.Any(we => we.ExerciseId == criteria.ExerciseId))
                .OrderByDescending(w => w.Date);

            var count = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((criteria.PageNumber - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginationList<WorkoutDto>(items.Select(w => w.ToDto()).ToList(), count, criteria.PageNumber, criteria.PageSize);
        }
    }
}
