using FluentValidation;
using ProjectManager.Application.Contracts.Commands.WorkPackages;

namespace ProjectManager.Application.Contracts.Validators.WorkPackages;

public class CreateWorkPackageCommandValidator : AbstractValidator<CreateWorkPackageCommand>
{
    public CreateWorkPackageCommandValidator()
    {
        RuleFor(x => x.ProjectId).GreaterThan(0).WithMessage("项目Id无效");
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("工作包编号不能为空")
            .MaximumLength(50);
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("工作包名称不能为空")
            .MaximumLength(200);
        RuleFor(x => x.Owner).MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}