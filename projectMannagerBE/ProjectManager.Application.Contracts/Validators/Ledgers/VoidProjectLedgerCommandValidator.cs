using FluentValidation;
using ProjectManager.Application.Contracts.Commands.Ledgers;

namespace ProjectManager.Application.Contracts.Validators.Ledgers;

public class VoidProjectLedgerCommandValidator : AbstractValidator<VoidProjectLedgerCommand>
{
    public VoidProjectLedgerCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("台账Id无效");
    }
}