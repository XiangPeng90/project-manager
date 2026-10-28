namespace ProjectManager.Application.Contracts.Dtos.Auth;

public class LoginInput
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}