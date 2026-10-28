using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Contracts.Dtos.WorkPackages;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Services;
using ProjectManager.Domain.Shared.Constants;
using ProjectManager.Domain.Shared.Enums;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.WorkPackages;

public class GetWorkPackageDetailQueryHandler : IRequestHandler<GetWorkPackageDetailQuery, WorkPackageDetailDto>
{
    private readonly IWorkPackageRepository _workPackageRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkPackageLedgerRepository _ledgerRepository;
    private readonly ICacheService _cache;

    public GetWorkPackageDetailQueryHandler(
        IWorkPackageRepository workPackageRepository,
        IProjectRepository projectRepository,
        IWorkPackageLedgerRepository ledgerRepository,
        ICacheService cache)
    {
        _workPackageRepository = workPackageRepository;
        _projectRepository = projectRepository;
        _ledgerRepository = ledgerRepository;
        _cache = cache;
    }

    public async Task<WorkPackageDetailDto> Handle(GetWorkPackageDetailQuery request, CancellationToken cancellationToken)
    {
        var version = await _cache.GetVersionAsync(CacheKeys.WorkPackageDetailVersion, cancellationToken);
        var cacheKey = $"{CacheKeys.WorkPackageDetail}:{request.Id}:v{version}";

        var cached = await _cache.GetAsync<WorkPackageDetailDto>(cacheKey, cancellationToken);
        if (cached != null) return cached;

        var wp = await _workPackageRepository.GetByIdAsync(request.Id, cancellationToken);
        if (wp == null)
            throw new BusinessException("工作包不存在");

        var project = await _projectRepository.GetByIdAsync(wp.ProjectId, cancellationToken);

        var ledgers = await _ledgerRepository.Query()
            .Where(l => l.WorkPackageId == request.Id && l.Status == LedgerStatus.Active)
            .OrderBy(l => l.YearMonth)
            .Select(l => new WorkPackageLedgerBriefDto
            {
                Id = l.Id,
                YearMonth = l.YearMonth,
                Cashflow = l.Cashflow,
                Funding = l.Funding,
                Status = (int)l.Status
            })
            .ToListAsync(cancellationToken);

        var dto = new WorkPackageDetailDto
        {
            Id = wp.Id,
            ProjectId = wp.ProjectId,
            ProjectName = project?.Name ?? string.Empty,
            Code = wp.Code,
            Name = wp.Name,
            Owner = wp.Owner,
            Description = wp.Description,
            Status = (int)wp.Status,
            Ledgers = ledgers
        };

        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(10), cancellationToken);
        return dto;
    }
}