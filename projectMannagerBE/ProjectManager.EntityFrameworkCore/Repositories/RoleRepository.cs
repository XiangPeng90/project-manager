using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.EntityFrameworkCore.Repositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(ProjectManagerDbContext dbContext) : base(dbContext) { }

    public Task<Role?> GetByCodeAsync(string code, CancellationToken ct = default)
        => DbSet.FirstOrDefaultAsync(r => r.Code == code, ct);
}