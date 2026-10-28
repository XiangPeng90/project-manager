using FluentValidation;
using ProjectManager.Application.Contracts.Commands.WorkPackages;

namespace ProjectManager.Application.Contracts.Validators.WorkPackages;

public class DisableWorkPackageCommandValidator : AbstractValidator<DisableWorkPackageCommand>
{
    public DisableWorkPackageCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("工作包Id无效");
    }
}