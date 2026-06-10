using Application.Helpers;
using Domain.Models;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories
{
    public interface IAsyncRepository<T> where T : class, IEntity<int>
    {
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TResult>> ProjectAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TResult>> ProjectAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<PaginationList<TResult>> GetPagedAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? predicate,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}
