using Application.Interfaces.Common;

namespace Infrastructure.Data
{
    public sealed class UnitOfWork(DataContext context) : IUnitOfWork
    {
        private readonly DataContext _context = context;

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
