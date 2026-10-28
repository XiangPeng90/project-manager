using FluentValidation;
using ProjectManager.Application.Contracts.Dtos.Ledgers;

namespace ProjectManager.Application.Contracts.Validators.Ledgers;

public class GetProjectLedgersQueryValidator : AbstractValidator<GetProjectLedgersQuery>
{
    public GetProjectLedgersQueryValidator()
    {
        RuleFor(x => x.ProjectId).GreaterThan(0).WithMessage("项目Id无效");
    }
}