using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Contracts.Dtos.WorkPackages;
using ProjectManager.Application.Contracts.Dtos;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.WorkPackages;

public class GetWorkPackageListQueryHandler : IRequestHandler<GetWorkPackageListQuery, PagedResult<WorkPackageDto>>
{
    private readonly IWorkPackageRepository _workPackageRepository;
    private readonly IProjectRepository _projectRepository;

    public GetWorkPackageListQueryHandler(
        IWorkPackageRepository workPackageRepository,
        IProjectRepository projectRepository)
    {
        _workPackageRepository = workPackageRepository;
        _projectRepository = projectRepository;
    }

    public async Task<PagedResult<WorkPackageDto>> Handle(GetWorkPackageListQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null)
            throw new BusinessException("项目不存在");

        var query = _workPackageRepository.Query()
            .Where(w => w.ProjectId == request.ProjectId);

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim();
            query = query.Where(w => w.Code.Contains(kw) || w.Name.Contains(kw));
        }

        if (request.Status.HasValue)
            query = query.Where(w => (int)w.Status == request.Status.Value);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(w => w.Code)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(w => new WorkPackageDto
            {
                Id = w.Id,
                ProjectId = w.ProjectId,
                Code = w.Code,
                Name = w.Name,
                Owner = w.Owner,
                Description = w.Description,
                Status = (int)w.Status,
                CreatedAt = w.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<WorkPackageDto>
        {
            Items = items,
            Total = total,
            Page = request.Page,
            Size = request.Size
        };
    }
}