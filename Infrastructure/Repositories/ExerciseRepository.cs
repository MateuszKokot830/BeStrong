using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ExerciseRepository(DataContext context) : IExerciseRepository
    {
        private readonly DataContext _context = context;

        public Task AddAsync(Exercise exercise, CancellationToken cancellationToken = default)
        {
            _context.Excercises.Add(exercise);
            return Task.CompletedTask;
        }
    }
}
