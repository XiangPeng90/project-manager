using MediatR;

namespace ProjectManager.Application.Contracts.Commands.Projects;

public class UpdateProjectCommand : IRequest<bool>
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Manager { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
}