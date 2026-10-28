using ProjectManager.Domain.Shared;
using ProjectManager.Domain.Shared.Enums;

namespace ProjectManager.Domain.Entities;

public class Project : AuditedEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Manager { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.Enabled;

}