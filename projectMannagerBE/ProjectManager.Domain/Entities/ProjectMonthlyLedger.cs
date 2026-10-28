using ProjectManager.Domain.Shared;
using ProjectManager.Domain.Shared.Enums;

namespace ProjectManager.Domain.Entities;

public class ProjectMonthlyLedger : AuditedEntity
{
    public long ProjectId { get; set; }
    public int YearMonth { get; set; }
    public decimal Cashflow { get; set; }
    public decimal Funding { get; set; }
    public string? Remark { get; set; }
    public LedgerStatus Status { get; set; } = LedgerStatus.Active;
}