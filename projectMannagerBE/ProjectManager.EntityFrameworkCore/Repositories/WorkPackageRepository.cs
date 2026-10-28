using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.EntityFrameworkCore.Repositories;

public class WorkPackageRepository : BaseRepository<WorkPackage>, IWorkPackageRepository
{
    public WorkPackageRepository(ProjectManagerDbContext dbContext) : base(dbContext) { }

    public Task<bool> CodeExistsAsync(long projectId, string code, long? excludeId = null, CancellationToken ct = default)
        => DbSet.AnyAsync(w => w.ProjectId == projectId && w.Code == code
            && (!excludeId.HasValue || w.Id != excludeId), ct);

    public Task<List<WorkPackage>> GetByProjectIdAsync(long projectId, CancellationToken ct = default)
        => DbSet.AsNoTracking()
            .Where(w => w.ProjectId == projectId)
            .OrderBy(w => w.Code)
            .ToListAsync(ct);
}