using FluentValidation;
using ProjectManager.Application.Contracts.Dtos.Projects;

namespace ProjectManager.Application.Contracts.Validators.Projects;

public class GetProjectListQueryValidator : AbstractValidator<GetProjectListQuery>
{
    public GetProjectListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Size).GreaterThan(0).LessThanOrEqualTo(100);
    }
}