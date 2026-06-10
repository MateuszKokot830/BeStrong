using Domain.Aggregates;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class WorkoutRepository(DataContext context) : BaseRepository<Workout>(context), IWorkoutRepository
    {
        protected override IQueryable<Workout> GetQueryable() =>
            _context.Workouts.Include(w => w.WorkoutExercises);

        public override async Task AddAsync(Workout workout, CancellationToken cancellationToken = default)
        {
            var wEx = workout.WorkoutExercises.ToList();
            if (wEx != null && wEx.Any())
            {
                _context.WorkoutExercises.AddRange(wEx);
            }
            workout.Date = DateTime.Now;
            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Workout>> GetUserWorkoutsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await GetQueryable()
                .Where(w => w.UserId == id)
                .OrderByDescending(w => w.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Exercise>> GetExercisesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Excercises.ToListAsync(cancellationToken);
        }

        public async Task CreateExerciseAsync(Exercise exercise, CancellationToken cancellationToken = default)
        {
            _context.Add<Exercise>(exercise);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
