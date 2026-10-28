using MediatR;

namespace ProjectManager.Application.Contracts.Commands.Projects;

public class DisableProjectCommand : IRequest<bool>
{
    public long Id { get; set; }
}