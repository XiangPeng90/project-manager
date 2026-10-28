using MediatR;

namespace ProjectManager.Application.Contracts.Dtos.Projects;

public class GetProjectListQuery : IRequest<PagedResult<ProjectDto>>
{
    public string? Keyword { get; set; }
    public int? Status { get; set; }
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}