namespace ProjectManager.Application.Contracts.Dtos.Projects;

public class ProjectDetailDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Manager { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public List<WorkPackageBriefDto> WorkPackages { get; set; } = new();
}

public class WorkPackageBriefDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Owner { get; set; }
    public int Status { get; set; }
}