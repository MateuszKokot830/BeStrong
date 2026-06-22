using Application.Interfaces.Repositories;
using Domain.Aggregates;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkoutRepository(DataContext context) : BaseRepository<Workout>(context), IWorkoutRepository
    {
        protected override IQueryable<Workout> GetQueryable() =>
            _context.Workouts
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Sets);
    }
}
