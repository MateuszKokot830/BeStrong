using Domain.Aggregates;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class WorkoutRepository(DataContext context) : BaseRepository<Workout>(context), IWorkoutRepository
    {
        public override async Task AddAsync(Workout workout)
        {
            var wEx = workout.WorkoutExercises.ToList();
            if (wEx != null && wEx.Any())
            {
                _context.WorkoutExercises.AddRange(wEx);
            }
            workout.Date = DateTime.Now;
            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Workout>> GetUserWorkoutsAsync(int id)
        {
            return await _context.Workouts.Include(w => w.WorkoutExercises)
                .Where(w => w.UserId == id)
                .OrderByDescending(w => w.Date)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Exercise>> GetExercisesAsync()
        {
            return await _context.Excercises.ToListAsync();
        }

        public async Task CreateExerciseAsync(Exercise exercise)
        {
            _context.Add<Exercise>(exercise);
            await _context.SaveChangesAsync();
        }
    }
}