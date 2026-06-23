using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ExerciseRepository(DataContext context) : IExerciseRepository
    {
        private readonly DataContext _context = context;

        public async Task<Exercise?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
            await _context.Excercises.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

        public Task AddAsync(Exercise exercise, CancellationToken cancellationToken = default)
        {
            _context.Excercises.Add(exercise);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Exercise exercise, CancellationToken cancellationToken = default)
        {
            _context.Entry(exercise).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Exercise exercise, CancellationToken cancellationToken = default)
        {
            _context.Excercises.Remove(exercise);
            return Task.CompletedTask;
        }
    }
}
