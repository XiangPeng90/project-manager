using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Repositories;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role?> GetByCodeAsync(string code, CancellationToken ct = default);
}