using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Shared.Enums;

namespace ProjectManager.EntityFrameworkCore.Repositories;

public class ProjectLedgerRepository : BaseRepository<ProjectMonthlyLedger>, IProjectLedgerRepository
{
    public ProjectLedgerRepository(ProjectManagerDbContext dbContext) : base(dbContext) { }

    public Task<ProjectMonthlyLedger?> GetActiveAsync(long projectId, int yearMonth, CancellationToken ct = default)
        => DbSet.FirstOrDefaultAsync(l => l.ProjectId == projectId
            && l.YearMonth == yearMonth
            && l.Status == LedgerStatus.Active, ct);

    public Task<List<ProjectMonthlyLedger>> GetByProjectAsync(long projectId, int? from = null, int? to = null, CancellationToken ct = default)
    {
        var query = DbSet.AsNoTracking()
            .Where(l => l.ProjectId == projectId && l.Status == LedgerStatus.Active);

        if (from.HasValue) query = query.Where(l => l.YearMonth >= from.Value);
        if (to.HasValue) query = query.Where(l => l.YearMonth <= to.Value);

        return query.OrderBy(l => l.YearMonth).ToListAsync(ct);
    }
}