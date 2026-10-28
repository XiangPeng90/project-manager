namespace ProjectManager.Application.Contracts.Dtos.Ledgers;

public class WorkPackageLedgerDto
{
    public long Id { get; set; }
    public long WorkPackageId { get; set; }
    public int YearMonth { get; set; }
    public decimal Cashflow { get; set; }
    public decimal Funding { get; set; }
    public string? Remark { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
}