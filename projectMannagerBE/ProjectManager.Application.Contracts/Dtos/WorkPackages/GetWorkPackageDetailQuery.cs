using MediatR;

namespace ProjectManager.Application.Contracts.Dtos.WorkPackages;

public class GetWorkPackageDetailQuery : IRequest<WorkPackageDetailDto>
{
    public long Id { get; set; }
}