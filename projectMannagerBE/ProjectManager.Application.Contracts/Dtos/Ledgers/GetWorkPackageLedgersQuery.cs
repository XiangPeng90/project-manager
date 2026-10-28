using MediatR;

namespace ProjectManager.Application.Contracts.Dtos.Ledgers;

public class GetWorkPackageLedgersQuery : IRequest<List<WorkPackageLedgerDto>>
{
    public long WorkPackageId { get; set; }
    public int? From { get; set; }
    public int? To { get; set; }
}