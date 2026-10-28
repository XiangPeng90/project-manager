using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Contracts.Commands.Auth;
using ProjectManager.Application.Contracts.Dtos.Auth;

namespace ProjectManager.HttpApi.Host.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public Task<LoginResultDto> Login([FromBody] LoginCommand command)
        => _mediator.Send(command);
}