using FluentValidation;
using ProjectManager.Application.Contracts.Dtos.WorkPackages;

namespace ProjectManager.Application.Contracts.Validators.WorkPackages;

public class GetWorkPackageListQueryValidator : AbstractValidator<GetWorkPackageListQuery>
{
    public GetWorkPackageListQueryValidator()
    {
        RuleFor(x => x.ProjectId).GreaterThan(0).WithMessage("项目Id无效");
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Size).GreaterThan(0).LessThanOrEqualTo(100);
    }
}