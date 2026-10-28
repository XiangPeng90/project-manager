using MediatR;
using ProjectManager.Application.Contracts.Dtos.Auth;

namespace ProjectManager.Application.Contracts.Commands.Auth;

public class LoginCommand : IRequest<LoginResultDto>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}