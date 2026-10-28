using FluentValidation;
using ProjectManager.Application.Contracts.Dtos.WorkPackages;

namespace ProjectManager.Application.Contracts.Validators.WorkPackages;

public class GetWorkPackageDetailQueryValidator : AbstractValidator<GetWorkPackageDetailQuery>
{
    public GetWorkPackageDetailQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("工作包Id无效");
    }
}