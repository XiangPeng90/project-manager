using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.EntityFrameworkCore.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ProjectManagerDbContext dbContext) : base(dbContext) { }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
        => DbSet.FirstOrDefaultAsync(u => u.Username == username, ct);

    public Task<List<string>> GetRoleCodesAsync(long userId, CancellationToken ct = default)
        => (from ur in DbContext.UserRoles
            join r in DbContext.Roles on ur.RoleId equals r.Id
            where ur.UserId == userId && r.IsActive
            select r.Code).ToListAsync(ct);
}