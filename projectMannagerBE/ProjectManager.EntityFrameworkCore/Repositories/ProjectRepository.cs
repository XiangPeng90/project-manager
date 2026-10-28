using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.EntityFrameworkCore.Repositories;

public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    public ProjectRepository(ProjectManagerDbContext dbContext) : base(dbContext) { }

    public Task<bool> CodeExistsAsync(string code, long? excludeId = null, CancellationToken ct = default)
        => DbSet.AnyAsync(p => p.Code == code && (!excludeId.HasValue || p.Id != excludeId), ct);
}