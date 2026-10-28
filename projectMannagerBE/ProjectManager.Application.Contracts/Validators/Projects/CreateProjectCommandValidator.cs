using FluentValidation;
using ProjectManager.Application.Contracts.Commands.Projects;

namespace ProjectManager.Application.Contracts.Validators.Projects;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("项目编号不能为空")
            .MaximumLength(50).WithMessage("项目编号不能超过50个字符");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("项目名称不能为空")
            .MaximumLength(200).WithMessage("项目名称不能超过200个字符");

        RuleFor(x => x.Manager)
            .MaximumLength(100).WithMessage("负责人不能超过100个字符");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("描述不能超过500个字符");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("结束日期不能早于开始日期");
    }
}