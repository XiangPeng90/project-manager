namespace ProjectManager.Application.Contracts.Dtos.WorkPackages;

public class WorkPackageDto
{
    public long Id { get; set; }
    public long ProjectId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Owner { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
}