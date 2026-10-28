using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.EntityFrameworkCore.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly ProjectManagerDbContext DbContext;
    protected readonly DbSet<T> DbSet;

    public BaseRepository(ProjectManagerDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<T>();
    }

    public Task<T?> GetByIdAsync(long id, CancellationToken ct = default)
        => DbSet.FindAsync(new object?[] { id }, ct).AsTask();

    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => DbSet.FirstOrDefaultAsync(predicate, ct);

    public Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
        => predicate == null
            ? DbSet.AsNoTracking().ToListAsync(ct)
            : DbSet.AsNoTracking().Where(predicate).ToListAsync(ct);

    public IQueryable<T> Query(bool asNoTracking = true)
        => asNoTracking ? DbSet.AsNoTracking() : DbSet;

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await DbSet.AddAsync(entity, ct);
        await DbContext.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        DbSet.Update(entity);
        await DbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        DbSet.Remove(entity);
        await DbContext.SaveChangesAsync(ct);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => DbContext.SaveChangesAsync(ct);
}