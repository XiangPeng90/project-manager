using MediatR;

namespace ProjectManager.Application.Contracts.Dtos.Projects;

public class GetProjectDetailQuery : IRequest<ProjectDetailDto>
{
    public long Id { get; set; }
}