using MediatR;

namespace ProjectManager.Application.Contracts.Commands.WorkPackages;

public class CreateWorkPackageCommand : IRequest<long>
{
    public long ProjectId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Owner { get; set; }
    public string? Description { get; set; }
}