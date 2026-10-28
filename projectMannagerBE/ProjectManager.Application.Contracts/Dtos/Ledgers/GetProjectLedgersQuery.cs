using MediatR;

namespace ProjectManager.Application.Contracts.Dtos.Ledgers;

public class GetProjectLedgersQuery : IRequest<List<ProjectLedgerDto>>
{
    public long ProjectId { get; set; }
    public int? From { get; set; }
    public int? To { get; set; }
}