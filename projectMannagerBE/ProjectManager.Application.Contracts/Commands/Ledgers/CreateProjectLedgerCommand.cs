using MediatR;

namespace ProjectManager.Application.Contracts.Commands.Ledgers;

public class CreateProjectLedgerCommand : IRequest<long>
{
    public long ProjectId { get; set; }
    public int YearMonth { get; set; }
    public decimal Cashflow { get; set; }
    public decimal Funding { get; set; }
    public string? Remark { get; set; }
}