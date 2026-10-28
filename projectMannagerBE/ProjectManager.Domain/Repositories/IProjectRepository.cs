using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Repositories;

public interface IProjectRepository : IBaseRepository<Project>
{
    Task<bool> CodeExistsAsync(string code, long? excludeId = null, CancellationToken ct = default);
}