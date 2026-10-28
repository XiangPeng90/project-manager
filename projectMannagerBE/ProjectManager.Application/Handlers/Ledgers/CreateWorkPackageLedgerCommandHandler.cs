using MediatR;
using ProjectManager.Application.Contracts.Commands.Ledgers;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Services;
using ProjectManager.Domain.Shared.Constants;
using ProjectManager.Domain.Shared.Enums;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.Ledgers;

public class CreateWorkPackageLedgerCommandHandler : IRequestHandler<CreateWorkPackageLedgerCommand, long>
{
    private readonly IWorkPackageLedgerRepository _ledgerRepository;
    private readonly IWorkPackageRepository _workPackageRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICacheService _cache;

    public CreateWorkPackageLedgerCommandHandler(
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

    public async Task<long> Handle(CreateWorkPackageLedgerCommand request, CancellationToken cancellationToken)
    {
        // 1. 工作包存在且未停用
        var wp = await _workPackageRepository.GetByIdAsync(request.WorkPackageId, cancellationToken);
        if (wp == null)
            throw new BusinessException("工作包不存在");
        if (wp.Status == ProjectStatus.Disabled)
            throw new BusinessException("工作包已停用，不允许新增台账");

        // 2. 所属项目未停用
        var project = await _projectRepository.GetByIdAsync(wp.ProjectId, cancellationToken);
        if (project == null)
            throw new BusinessException("所属项目不存在");
        if (project.Status == ProjectStatus.Disabled)
            throw new BusinessException("所属项目已停用，不允许新增工作包台账");

        // 3. 唯一性
        var exists = await _ledgerRepository.GetActiveAsync(request.WorkPackageId, request.YearMonth, cancellationToken);
        if (exists != null)
            throw new BusinessException($"{request.YearMonth} 月份的工作包台账已存在，请直接编辑");

        var ledger = new WorkPackageMonthlyLedger
        {
            WorkPackageId = request.WorkPackageId,
            YearMonth = request.YearMonth,
            Cashflow = request.Cashflow,
            Funding = request.Funding,
            Remark = request.Remark?.Trim(),
            Status = LedgerStatus.Active
        };

        await _ledgerRepository.AddAsync(ledger, cancellationToken);

        // 清缓存：工作包报表 + 项目报表（项目报表可能汇总了工作包数据）
        await _cache.IncreaseVersionAsync(CacheKeys.ReportWorkPackageVersion, cancellationToken);
        await _cache.IncreaseVersionAsync(CacheKeys.ReportProjectVersion, cancellationToken);
        await _cache.IncreaseVersionAsync(CacheKeys.WorkPackageDetailVersion, cancellationToken);
        
        return ledger.Id;
    }
}