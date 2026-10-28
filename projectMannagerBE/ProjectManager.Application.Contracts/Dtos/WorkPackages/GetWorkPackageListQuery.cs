using MediatR;

namespace ProjectManager.Application.Contracts.Dtos.WorkPackages;

public class GetWorkPackageListQuery : IRequest<PagedResult<WorkPackageDto>>
{
    public long ProjectId { get; set; }
    public string? Keyword { get; set; }
    public int? Status { get; set; }
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}