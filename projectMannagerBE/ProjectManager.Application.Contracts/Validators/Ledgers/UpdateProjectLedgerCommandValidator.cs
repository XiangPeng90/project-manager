using FluentValidation;
using ProjectManager.Application.Contracts.Commands.Ledgers;

namespace ProjectManager.Application.Contracts.Validators.Ledgers;

public class UpdateProjectLedgerCommandValidator : AbstractValidator<UpdateProjectLedgerCommand>
{
    public UpdateProjectLedgerCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("台账Id无效");
        RuleFor(x => x.Remark).MaximumLength(200);
    }
}