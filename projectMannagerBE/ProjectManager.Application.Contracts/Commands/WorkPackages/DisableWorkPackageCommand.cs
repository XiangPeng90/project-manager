using MediatR;

namespace ProjectManager.Application.Contracts.Commands.WorkPackages;

public class DisableWorkPackageCommand : IRequest<bool>
{
    public long Id { get; set; }
}