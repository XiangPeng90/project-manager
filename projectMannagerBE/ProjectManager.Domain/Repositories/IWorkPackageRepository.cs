using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Repositories;

public interface IWorkPackageRepository : IBaseRepository<WorkPackage>
{
    Task<bool> CodeExistsAsync(long projectId, string code, long? excludeId = null, CancellationToken ct = default);
    Task<List<WorkPackage>> GetByProjectIdAsync(long projectId, CancellationToken ct = default);
}