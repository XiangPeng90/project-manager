using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Contracts.Dtos.Projects;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Services;
using ProjectManager.Domain.Shared.Constants;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.Projects;

public class GetProjectDetailQueryHandler : IRequestHandler<GetProjectDetailQuery, ProjectDetailDto>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkPackageRepository _workPackageRepository;
    private readonly ICacheService _cache;

    public GetProjectDetailQueryHandler(
        IProjectRepository projectRepository,
        IWorkPackageRepository workPackageRepository,
        ICacheService cache)
    {
        _projectRepository = projectRepository;
        _workPackageRepository = workPackageRepository;
        _cache = cache;
    }

    public async Task<ProjectDetailDto> Handle(GetProjectDetailQuery request, CancellationToken cancellationToken)
    {
        var version = await _cache.GetVersionAsync(CacheKeys.ProjectDetailVersion, cancellationToken);
        var cacheKey = $"{CacheKeys.ProjectDetail}:{request.Id}:v{version}";

        var cached = await _cache.GetAsync<ProjectDetailDto>(cacheKey, cancellationToken);
        if (cached != null) return cached;

        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken);
        if (project == null)
            throw new BusinessException("项目不存在");

        var workPackages = await _workPackageRepository.Query()
            .Where(w => w.ProjectId == request.Id)
            .OrderBy(w => w.Code)
            .Select(w => new WorkPackageBriefDto
            {
                Id = w.Id,
                Code = w.Code,
                Name = w.Name,
                Owner = w.Owner,
                Status = (int)w.Status
            })
            .ToListAsync(cancellationToken);

        var dto = new ProjectDetailDto
        {
            Id = project.Id,
            Code = project.Code,
            Name = project.Name,
            Manager = project.Manager,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Description = project.Description,
            Status = (int)project.Status,
            WorkPackages = workPackages
        };

        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(10), cancellationToken);
        return dto;
    }
}