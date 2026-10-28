using MediatR;
using ProjectManager.Application.Contracts.Commands.Ledgers;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Services;
using ProjectManager.Domain.Shared.Constants;
using ProjectManager.Domain.Shared.Enums;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.Ledgers;

public class CreateProjectLedgerCommandHandler : IRequestHandler<CreateProjectLedgerCommand, long>
{
    private readonly IProjectLedgerRepository _ledgerRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICacheService _cache;

    public CreateProjectLedgerCommandHandler(
        IProjectLedgerRepository ledgerRepository,
        IProjectRepository projectRepository,
        ICacheService cache)
    {
        _ledgerRepository = ledgerRepository;
        _projectRepository = projectRepository;
        _cache = cache;
    }

    public async Task<long> Handle(CreateProjectLedgerCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null)
            throw new BusinessException("项目不存在");
        if (project.Status == ProjectStatus.Disabled)
            throw new BusinessException("项目已停用，不允许新增台账");

        var exists = await _ledgerRepository.GetActiveAsync(request.ProjectId, request.YearMonth, cancellationToken);
        if (exists != null)
            throw new BusinessException($"{request.YearMonth} 月份的项目台账已存在，请直接编辑");

        var ledger = new ProjectMonthlyLedger
        {
            ProjectId = request.ProjectId,
            YearMonth = request.YearMonth,
            Cashflow = request.Cashflow,
            Funding = request.Funding,
            Remark = request.Remark?.Trim(),
            Status = LedgerStatus.Active
        };

        await _ledgerRepository.AddAsync(ledger, cancellationToken);

        // 清项目报表缓存
        await _cache.IncreaseVersionAsync(CacheKeys.ReportProjectVersion, cancellationToken);

        return ledger.Id;
    }
}