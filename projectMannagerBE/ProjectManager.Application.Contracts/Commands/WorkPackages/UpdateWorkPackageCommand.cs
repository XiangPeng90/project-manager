using MediatR;

namespace ProjectManager.Application.Contracts.Commands.WorkPackages;

public class UpdateWorkPackageCommand : IRequest<bool>
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Owner { get; set; }
    public string? Description { get; set; }
}