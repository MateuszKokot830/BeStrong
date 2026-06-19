using Application.Dto.Exercise;
using Application.Interfaces.Searchers;
using Application.Mappings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Searchers
{
    public sealed class ExerciseSearcher(DataContext context) : IExerciseSearcher
    {
        private readonly DataContext _context = context;

        public async Task<IReadOnlyList<ExerciseDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var exercises = await _context.Excercises
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return exercises.Select(e => e.ToDto()).ToList();
        }
    }
}
