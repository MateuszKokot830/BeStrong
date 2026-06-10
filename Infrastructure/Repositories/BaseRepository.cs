using Application.Helpers;
using Application.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T>(DataContext context) : IAsyncRepository<T>
        where T : class, IEntity<int>
    {
        protected readonly DataContext _context = context;

        protected virtual IQueryable<T> GetQueryable() => _context.Set<T>();

        public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await GetQueryable().AsNoTracking().ToListAsync(cancellationToken);
        }

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await GetQueryable()
                .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TResult>> ProjectAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .Select(selector)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TResult>> ProjectAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .Where(predicate)
                .Select(selector)
                .ToListAsync(cancellationToken);
        }

        public async Task<PaginationList<TResult>> GetPagedAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? predicate,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Set<T>().AsNoTracking();

            if (predicate is not null)
                query = query.Where(predicate);

            return await PaginationList<TResult>.CreateAsync(
                query.Select(selector), pageNumber, pageSize, cancellationToken);
        }
    }
}
