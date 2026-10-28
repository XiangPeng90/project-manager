using FluentValidation;
using ProjectManager.Application.Contracts.Commands.WorkPackages;

namespace ProjectManager.Application.Contracts.Validators.WorkPackages;

public class UpdateWorkPackageCommandValidator : AbstractValidator<UpdateWorkPackageCommand>
{
    public UpdateWorkPackageCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("工作包Id无效");
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Owner).MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}