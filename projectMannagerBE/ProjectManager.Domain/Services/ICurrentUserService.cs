namespace ProjectManager.Domain.Services;

public interface ICurrentUserService
{
    long? UserId { get; }
    string? UserName { get; }
    IReadOnlyList<string> Roles { get; }
    bool IsInRole(string role);
}