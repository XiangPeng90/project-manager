using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Contracts.Dtos.Projects;
using ProjectManager.Application.Contracts.Dtos;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.Application.Handlers.Projects;

public class GetProjectListQueryHandler : IRequestHandler<GetProjectListQuery, PagedResult<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectListQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<PagedResult<ProjectDto>> Handle(GetProjectListQuery request, CancellationToken cancellationToken)
    {
        var query = _projectRepository.Query();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim();
            query = query.Where(p => p.Code.Contains(kw) || p.Name.Contains(kw));
        }

        if (request.Status.HasValue)
            query = query.Where(p => (int)p.Status == request.Status.Value);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(p => p.Id)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Manager = p.Manager,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Description = p.Description,
                Status = (int)p.Status,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<ProjectDto>
        {
            Items = items,
            Total = total,
            Page = request.Page,
            Size = request.Size
        };
    }
}