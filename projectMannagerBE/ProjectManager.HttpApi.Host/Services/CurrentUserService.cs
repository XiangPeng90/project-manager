using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ProjectManager.Domain.Services;

namespace ProjectManager.HttpApi.Host.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public long? UserId
    {
        get
        {
            var sub = User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                      ?? User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(sub, out var id) ? id : null;
        }
    }

    public string? UserName
        => User?.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value
           ?? User?.FindFirst(ClaimTypes.Name)?.Value;

    public IReadOnlyList<string> Roles
        => User?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList()
           ?? new List<string>();

    public bool IsInRole(string role)
        => Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
}