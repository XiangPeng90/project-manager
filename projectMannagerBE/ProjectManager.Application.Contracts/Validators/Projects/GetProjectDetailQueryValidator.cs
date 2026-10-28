using FluentValidation;
using ProjectManager.Application.Contracts.Dtos.Projects;

namespace ProjectManager.Application.Contracts.Validators.Projects;

public class GetProjectDetailQueryValidator : AbstractValidator<GetProjectDetailQuery>
{
    public GetProjectDetailQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("项目Id无效");
    }
}