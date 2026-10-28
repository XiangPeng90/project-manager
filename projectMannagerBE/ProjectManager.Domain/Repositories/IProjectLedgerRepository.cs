using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Repositories;

public interface IProjectLedgerRepository : IBaseRepository<ProjectMonthlyLedger>
{
    Task<ProjectMonthlyLedger?> GetActiveAsync(long projectId, int yearMonth, CancellationToken ct = default);
    Task<List<ProjectMonthlyLedger>> GetByProjectAsync(long projectId, int? from = null, int? to = null, CancellationToken ct = default);
}