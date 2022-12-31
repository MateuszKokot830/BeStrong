using Domain.Aggregates;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class WorkoutRepository  : BaseRepository<Workout>, IWorkoutRepository
    {
        public WorkoutRepository(DataContext context) : base(context)
        {
        }

        public override async Task AddAsync(Workout workout)
        {
            var wEx = workout.WorkoutExercises.ToList();
            if (wEx!= null && wEx.Any())
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