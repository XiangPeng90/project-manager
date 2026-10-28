using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Contracts.Services;

public interface IJwtTokenService
{
    (string Token, DateTime ExpireAt) Generate(User user, List<string> roleCodes);
}