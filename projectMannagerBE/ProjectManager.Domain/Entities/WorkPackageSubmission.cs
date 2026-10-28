namespace ProjectManager.Domain.Entities;

/// <summary>
/// 工作包提交汇总留痕。注意：这张表是只增不改的日志表，
/// 不需要 UpdatedAt/UpdatedBy/IsDeleted，所以不继承 AuditedEntity。
/// </summary>
public class WorkPackageSubmission
{
    public long Id { get; set; }
    public long WorkPackageId { get; set; }
    public long ProjectId { get; set; }
    public int YearMonth { get; set; }
    public decimal Cashflow { get; set; }
    public decimal Funding { get; set; }
    public DateTime SubmittedAt { get; set; }
    public string? SubmittedBy { get; set; }
    public string? Remark { get; set; }
}