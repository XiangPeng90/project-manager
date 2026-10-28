namespace ProjectManager.Application.Contracts.Dtos.WorkPackages;

public class WorkPackageDetailDto
{
    public long Id { get; set; }
    public long ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Owner { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public List<WorkPackageLedgerBriefDto> Ledgers { get; set; } = new();
}

public class WorkPackageLedgerBriefDto
{
    public long Id { get; set; }
    public int YearMonth { get; set; }
    public decimal Cashflow { get; set; }
    public decimal Funding { get; set; }
    public int Status { get; set; }
}