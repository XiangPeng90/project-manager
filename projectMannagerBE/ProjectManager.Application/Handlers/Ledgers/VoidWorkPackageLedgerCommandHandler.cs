using MediatR;
using ProjectManager.Application.Contracts.Commands.Ledgers;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Services;
using ProjectManager.Domain.Shared.Constants;
using ProjectManager.Domain.Shared.Enums;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.Ledgers;

public class VoidWorkPackageLedgerCommandHandler : IRequestHandler<VoidWorkPackageLedgerCommand, bool>
{
    private readonly IWorkPackageLedgerRepository _ledgerRepository;
    private readonly IWorkPackageRepository _workPackageRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICacheService _cache;

    public VoidWorkPackageLedgerCommandHandler(
        IWorkPackageLedgerRepository ledgerRepository,
        IWorkPackageRepository workPackageRepository,
        IProjectRepository projectRepository,
        ICacheService cache)
    {
        _ledgerRepository = ledgerRepository;
        _workPackageRepository = workPackageRepository;
        _projectRepository = projectRepository;
        _cache = cache;
    }

    public async Task<bool> Handle(VoidWorkPackageLedgerCommand request, CancellationToken cancellationToken)
    {
        var ledger = await _ledgerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (ledger == null)
            throw new BusinessException("台账不存在");
        if (ledger.Status != LedgerStatus.Active)
            return true;

        var wp = await _workPackageRepository.GetByIdAsync(ledger.WorkPackageId, cancellationToken);
        if (wp == null)
            throw new BusinessException("工作包不存在");
        if (wp.Status == ProjectStatus.Disabled)
            throw new BusinessException("工作包已停用，不允许作废台账");

        var project = await _projectRepository.GetByIdAsync(wp.ProjectId, cancellationToken);
        if (project == null)
            throw new BusinessException("所属项目不存在");
        if (project.Status == ProjectStatus.Disabled)
            throw new BusinessException("所属项目已停用，不允许作废工作包台账");

        ledger.Status = LedgerStatus.Void;
        await _ledgerRepository.UpdateAsync(ledger, cancellationToken);

        await _cache.IncreaseVersionAsync(CacheKeys.ReportWorkPackageVersion, cancellationToken);
        await _cache.IncreaseVersionAsync(CacheKeys.ReportProjectVersion, cancellationToken);
        await _cache.IncreaseVersionAsync(CacheKeys.WorkPackageDetailVersion, cancellationToken);
        return true;
    }
}