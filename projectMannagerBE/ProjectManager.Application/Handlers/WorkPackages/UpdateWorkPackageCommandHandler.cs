using MediatR;
using ProjectManager.Application.Contracts.Commands.WorkPackages;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Services;
using ProjectManager.Domain.Shared.Constants;
using ProjectManager.Domain.Shared.Enums;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.WorkPackages;

public class UpdateWorkPackageCommandHandler : IRequestHandler<UpdateWorkPackageCommand, bool>
{
    private readonly IWorkPackageRepository _workPackageRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICacheService _cache;

    public UpdateWorkPackageCommandHandler(
        IWorkPackageRepository workPackageRepository,
        IProjectRepository projectRepository,
        ICacheService cache)
    {
        _workPackageRepository = workPackageRepository;
        _projectRepository = projectRepository;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateWorkPackageCommand request, CancellationToken cancellationToken)
    {
        var wp = await _workPackageRepository.GetByIdAsync(request.Id, cancellationToken);
        if (wp == null)
            throw new BusinessException("工作包不存在");

        // 1. 工作包本身停用不允许改
        if (wp.Status == ProjectStatus.Disabled)
            throw new BusinessException("工作包已停用，不允许修改");

        // 2. 所属项目停用也不允许改
        var project = await _projectRepository.GetByIdAsync(wp.ProjectId, cancellationToken);
        if (project == null)
            throw new BusinessException("所属项目不存在");
        if (project.Status == ProjectStatus.Disabled)
            throw new BusinessException("所属项目已停用，不允许修改工作包");

        // 3. 编号唯一（项目内，排除自己）
        var exists = await _workPackageRepository.CodeExistsAsync(wp.ProjectId, request.Code, request.Id, cancellationToken);
        if (exists)
            throw new BusinessException($"工作包编号 {request.Code} 在该项目下已存在");

        wp.Code = request.Code.Trim();
        wp.Name = request.Name.Trim();
        wp.Owner = request.Owner?.Trim();
        wp.Description = request.Description?.Trim();

        await _workPackageRepository.UpdateAsync(wp, cancellationToken);

        // 4. 清缓存：工作包详情 + 项目详情（清单里工作包信息变了）
        await _cache.IncreaseVersionAsync(CacheKeys.WorkPackageDetailVersion, cancellationToken);
        await _cache.IncreaseVersionAsync(CacheKeys.ProjectDetailVersion, cancellationToken);

        return true;
    }
}