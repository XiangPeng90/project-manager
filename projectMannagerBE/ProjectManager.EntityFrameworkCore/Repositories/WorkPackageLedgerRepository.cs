using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Shared.Enums;

namespace ProjectManager.EntityFrameworkCore.Repositories;

public class WorkPackageLedgerRepository : BaseRepository<WorkPackageMonthlyLedger>, IWorkPackageLedgerRepository
{
    public WorkPackageLedgerRepository(ProjectManagerDbContext dbContext) : base(dbContext) { }

    public Task<WorkPackageMonthlyLedger?> GetActiveAsync(long workPackageId, int yearMonth, CancellationToken ct = default)
        => DbSet.FirstOrDefaultAsync(l => l.WorkPackageId == workPackageId
            && l.YearMonth == yearMonth
            && l.Status == LedgerStatus.Active, ct);

    public Task<List<WorkPackageMonthlyLedger>> GetByWorkPackageAsync(long workPackageId, int? from = null, int? to = null, CancellationToken ct = default)
    {
        var query = DbSet.AsNoTracking()
            .Where(l => l.WorkPackageId == workPackageId && l.Status == LedgerStatus.Active);

        if (from.HasValue) query = query.Where(l => l.YearMonth >= from.Value);
        if (to.HasValue) query = query.Where(l => l.YearMonth <= to.Value);

        return query.OrderBy(l => l.YearMonth).ToListAsync(ct);
    }
}