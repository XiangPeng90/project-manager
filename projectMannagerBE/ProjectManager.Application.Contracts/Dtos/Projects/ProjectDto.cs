namespace ProjectManager.Application.Contracts.Dtos.Projects;

public class ProjectDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Manager { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
}