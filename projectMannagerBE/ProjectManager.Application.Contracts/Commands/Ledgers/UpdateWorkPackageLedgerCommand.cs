using MediatR;

namespace ProjectManager.Application.Contracts.Commands.Ledgers;

public class UpdateWorkPackageLedgerCommand : IRequest<bool>
{
    public long Id { get; set; }
    public decimal Cashflow { get; set; }
    public decimal Funding { get; set; }
    public string? Remark { get; set; }
}