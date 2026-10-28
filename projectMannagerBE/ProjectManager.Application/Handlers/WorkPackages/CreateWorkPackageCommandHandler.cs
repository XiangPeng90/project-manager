using MediatR;
using ProjectManager.Application.Contracts.Commands.WorkPackages;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Services;
using ProjectManager.Domain.Shared.Constants;
using ProjectManager.Domain.Shared.Enums;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.WorkPackages;

public class CreateWorkPackageCommandHandler : IRequestHandler<CreateWorkPackageCommand, long>
{
    private readonly IWorkPackageRepository _workPackageRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICacheService _cache;

    public CreateWorkPackageCommandHandler(
        IWorkPackageRepository workPackageRepository,
        IProjectRepository projectRepository,
        ICacheService cache)
    {
        _workPackageRepository = workPackageRepository;
        _projectRepository = projectRepository;
        _cache = cache;
    }

    public async Task<long> Handle(CreateWorkPackageCommand request, CancellationToken cancellationToken)
    {
        // 1. 校验项目存在且未停用
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null)
            throw new BusinessException("项目不存在");
        if (project.Status == ProjectStatus.Disabled)
            throw new BusinessException("项目已停用，不允许新增工作包");

        // 2. 编号唯一（项目内）
        var exists = await _workPackageRepository.CodeExistsAsync(request.ProjectId, request.Code, null, cancellationToken);
        if (exists)
            throw new BusinessException($"工作包编号 {request.Code} 在该项目下已存在");

        var wp = new WorkPackage
        {
            ProjectId = request.ProjectId,
            Code = request.Code.Trim(),
            Name = request.Name.Trim(),
            Owner = request.Owner?.Trim(),
            Description = request.Description?.Trim(),
            Status = ProjectStatus.Enabled
        };

        await _workPackageRepository.AddAsync(wp, cancellationToken);

        // 3. 清项目详情缓存（工作包清单变了）
        await _cache.IncreaseVersionAsync(CacheKeys.ProjectDetailVersion, cancellationToken);

        return wp.Id;
    }
}