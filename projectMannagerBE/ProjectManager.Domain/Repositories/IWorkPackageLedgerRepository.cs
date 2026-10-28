using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Repositories;

public interface IWorkPackageLedgerRepository : IBaseRepository<WorkPackageMonthlyLedger>
{
    Task<WorkPackageMonthlyLedger?> GetActiveAsync(long workPackageId, int yearMonth, CancellationToken ct = default);
    Task<List<WorkPackageMonthlyLedger>> GetByWorkPackageAsync(long workPackageId, int? from = null, int? to = null, CancellationToken ct = default);
}