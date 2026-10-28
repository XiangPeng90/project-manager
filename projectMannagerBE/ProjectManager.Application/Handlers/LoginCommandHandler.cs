using MediatR;
using ProjectManager.Application.Contracts.Commands.Auth;
using ProjectManager.Application.Contracts.Dtos.Auth;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Shared.Exceptions;
using ProjectManager.Application.Contracts.Services;

namespace ProjectManager.Application.Handlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user == null || !user.Status)
            throw new BusinessException("用户名或密码错误");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new BusinessException("用户名或密码错误");

        var roleCodes = await _userRepository.GetRoleCodesAsync(user.Id, cancellationToken);
        var (token, expireAt) = _jwtTokenService.Generate(user, roleCodes);

        return new LoginResultDto
        {
            Token = token,
            DisplayName = user.DisplayName,
            Roles = roleCodes,
            ExpireAt = expireAt
        };
    }
}