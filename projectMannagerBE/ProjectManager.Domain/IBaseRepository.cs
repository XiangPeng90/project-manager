using System.Linq.Expressions;

namespace ProjectManager.Domain.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(long id, CancellationToken ct = default);

    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

    Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);

    IQueryable<T> Query(bool asNoTracking = true);

    Task<T> AddAsync(T entity, CancellationToken ct = default);

    Task UpdateAsync(T entity, CancellationToken ct = default);

    Task DeleteAsync(T entity, CancellationToken ct = default);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}