using MediatR;
using ProjectManager.Application.Contracts.Commands.WorkPackages;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Services;
using ProjectManager.Domain.Shared.Constants;
using ProjectManager.Domain.Shared.Enums;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.WorkPackages;

public class DisableWorkPackageCommandHandler : IRequestHandler<DisableWorkPackageCommand, bool>
{
    private readonly IWorkPackageRepository _workPackageRepository;
    private readonly ICacheService _cache;

    public DisableWorkPackageCommandHandler(
        IWorkPackageRepository workPackageRepository,
        ICacheService cache)
    {
        _workPackageRepository = workPackageRepository;
        _cache = cache;
    }

    public async Task<bool> Handle(DisableWorkPackageCommand request, CancellationToken cancellationToken)
    {
        var wp = await _workPackageRepository.GetByIdAsync(request.Id, cancellationToken);
        if (wp == null)
            throw new BusinessException("工作包不存在");

        if (wp.Status == ProjectStatus.Disabled)
            return true; // 幂等

        wp.Status = ProjectStatus.Disabled;
        await _workPackageRepository.UpdateAsync(wp, cancellationToken);

        // 清缓存：工作包详情 + 项目详情
        await _cache.IncreaseVersionAsync(CacheKeys.WorkPackageDetailVersion, cancellationToken);
        await _cache.IncreaseVersionAsync(CacheKeys.ProjectDetailVersion, cancellationToken);

        return true;
    }
}