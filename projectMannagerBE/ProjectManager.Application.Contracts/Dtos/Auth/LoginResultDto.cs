namespace ProjectManager.Application.Contracts.Dtos.Auth;

public class LoginResultDto
{
    public string Token { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public DateTime ExpireAt { get; set; }
}