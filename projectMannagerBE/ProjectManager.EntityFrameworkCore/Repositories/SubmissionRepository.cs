using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.EntityFrameworkCore.Repositories;

public class SubmissionRepository : BaseRepository<WorkPackageSubmission>, ISubmissionRepository
{
    public SubmissionRepository(ProjectManagerDbContext dbContext) : base(dbContext) { }

    public Task<WorkPackageSubmission?> GetLatestAsync(long workPackageId, int yearMonth, CancellationToken ct = default)
        => DbSet.AsNoTracking()
            .Where(s => s.WorkPackageId == workPackageId && s.YearMonth == yearMonth)
            .OrderByDescending(s => s.SubmittedAt)
            .FirstOrDefaultAsync(ct);

    public Task<List<WorkPackageSubmission>> GetByWorkPackageAsync(long workPackageId, CancellationToken ct = default)
        => DbSet.AsNoTracking()
            .Where(s => s.WorkPackageId == workPackageId)
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync(ct);
}