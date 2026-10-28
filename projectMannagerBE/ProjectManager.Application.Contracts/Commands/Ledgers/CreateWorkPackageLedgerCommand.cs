using MediatR;

namespace ProjectManager.Application.Contracts.Commands.Ledgers;

public class CreateWorkPackageLedgerCommand : IRequest<long>
{
    public long WorkPackageId { get; set; }
    public int YearMonth { get; set; }
    public decimal Cashflow { get; set; }
    public decimal Funding { get; set; }
    public string? Remark { get; set; }
}