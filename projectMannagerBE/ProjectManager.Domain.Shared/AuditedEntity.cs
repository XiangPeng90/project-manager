namespace ProjectManager.Domain.Shared;

/// <summary>
/// 审计基类：所有实体继承，统一包含创建/更新/软删字段
/// </summary>
public abstract class AuditedEntity
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
}
