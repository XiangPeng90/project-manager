using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<List<string>> GetRoleCodesAsync(long userId, CancellationToken ct = default);
}