using Application.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T>(DataContext context) : IRepository<T>
        where T : class, IEntity<int>
    {
        protected readonly DataContext _context = context;

        protected virtual IQueryable<T> GetQueryable() => _context.Set<T>();

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await GetQueryable()
                .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public virtual Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Add(entity);
            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }
    }
}
