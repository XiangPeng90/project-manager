using FluentValidation;
using ProjectManager.Application.Contracts.Commands.Projects;

namespace ProjectManager.Application.Contracts.Validators.Projects;

public class DisableProjectCommandValidator : AbstractValidator<DisableProjectCommand>
{
    public DisableProjectCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("项目Id无效");
    }
}