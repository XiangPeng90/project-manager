using MediatR;
using ProjectManager.Application.Contracts.Commands.Ledgers;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Services;
using ProjectManager.Domain.Shared.Constants;
using ProjectManager.Domain.Shared.Enums;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.Ledgers;

public class UpdateProjectLedgerCommandHandler : IRequestHandler<UpdateProjectLedgerCommand, bool>
{
    private readonly IProjectLedgerRepository _ledgerRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICacheService _cache;

    public UpdateProjectLedgerCommandHandler(
        IProjectLedgerRepository ledgerRepository,
        IProjectRepository projectRepository,
        ICacheService cache)
    {
        _ledgerRepository = ledgerRepository;
        _projectRepository = projectRepository;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateProjectLedgerCommand request, CancellationToken cancellationToken)
    {
        var ledger = await _ledgerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (ledger == null)
            throw new BusinessException("台账不存在");
        if (ledger.Status != LedgerStatus.Active)
            throw new BusinessException("台账已作废，不允许修改");

        var project = await _projectRepository.GetByIdAsync(ledger.ProjectId, cancellationToken);
        if (project == null)
            throw new BusinessException("项目不存在");
        if (project.Status == ProjectStatus.Disabled)
            throw new BusinessException("项目已停用，不允许修改台账");

        ledger.Cashflow = request.Cashflow;
        ledger.Funding = request.Funding;
        ledger.Remark = request.Remark?.Trim();

        await _ledgerRepository.UpdateAsync(ledger, cancellationToken);
        await _cache.IncreaseVersionAsync(CacheKeys.ReportProjectVersion, cancellationToken);

        return true;
    }
}