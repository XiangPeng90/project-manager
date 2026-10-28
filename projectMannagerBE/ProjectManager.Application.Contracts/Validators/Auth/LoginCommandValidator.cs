using FluentValidation;
using ProjectManager.Application.Contracts.Commands.Auth;

namespace ProjectManager.Application.Contracts.Validators.Auth;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("用户名不能为空");
        RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空");
    }
}