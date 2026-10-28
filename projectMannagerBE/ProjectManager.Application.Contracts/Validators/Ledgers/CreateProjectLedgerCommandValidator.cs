using FluentValidation;
using ProjectManager.Application.Contracts.Commands.Ledgers;

namespace ProjectManager.Application.Contracts.Validators.Ledgers;

public class CreateProjectLedgerCommandValidator : AbstractValidator<CreateProjectLedgerCommand>
{
    public CreateProjectLedgerCommandValidator()
    {
        RuleFor(x => x.ProjectId).GreaterThan(0).WithMessage("项目Id无效");
        RuleFor(x => x.YearMonth)
            .InclusiveBetween(190001, 999912).WithMessage("年月格式无效，应为 202401 形式");
        RuleFor(x => x.Remark).MaximumLength(200);
    }
}