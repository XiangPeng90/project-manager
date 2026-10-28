using FluentValidation;
using ProjectManager.Application.Contracts.Commands.Submissions;

namespace ProjectManager.Application.Contracts.Dtos.Submissions;

public class SubmitWorkPackageCommandValidator : AbstractValidator<SubmitWorkPackageCommand>
{
    public SubmitWorkPackageCommandValidator()
    {
        RuleFor(x => x.WorkPackageId).GreaterThan(0).WithMessage("工作包Id无效");
        RuleFor(x => x.YearMonth)
            .InclusiveBetween(190001, 999912).WithMessage("年月格式无效，应为 202401 形式");
        RuleFor(x => x.Remark).MaximumLength(200);
    }
}