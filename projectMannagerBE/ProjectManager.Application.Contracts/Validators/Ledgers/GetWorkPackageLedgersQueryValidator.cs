using FluentValidation;
using ProjectManager.Application.Contracts.Dtos.Ledgers;

namespace ProjectManager.Application.Contracts.Validators.Ledgers;

public class GetWorkPackageLedgersQueryValidator : AbstractValidator<GetWorkPackageLedgersQuery>
{
    public GetWorkPackageLedgersQueryValidator()
    {
        RuleFor(x => x.WorkPackageId).GreaterThan(0).WithMessage("工作包Id无效");
    }
}