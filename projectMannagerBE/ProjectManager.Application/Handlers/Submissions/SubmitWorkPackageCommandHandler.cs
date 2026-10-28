using MediatR;
using ProjectManager.Application.Contracts.Commands.Submissions;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Services;
using ProjectManager.Domain.Shared.Constants;
using ProjectManager.Domain.Shared.Enums;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.Submissions;

public class SubmitWorkPackageCommandHandler : IRequestHandler<SubmitWorkPackageCommand, bool>
{
    private readonly IWorkPackageRepository _workPackageRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkPackageLedgerRepository _wpLedgerRepository;
    private readonly IProjectLedgerRepository _projectLedgerRepository;
    private readonly ISubmissionRepository _submissionRepository;
    private readonly ICacheService _cache;
    private readonly ICurrentUserService _currentUser;

    public SubmitWorkPackageCommandHandler(
        IWorkPackageRepository workPackageRepository,
        IProjectRepository projectRepository,
        IWorkPackageLedgerRepository wpLedgerRepository,
        IProjectLedgerRepository projectLedgerRepository,
        ISubmissionRepository submissionRepository,
        ICacheService cache,
        ICurrentUserService currentUser)
    {
        _workPackageRepository = workPackageRepository;
        _projectRepository = projectRepository;
        _wpLedgerRepository = wpLedgerRepository;
        _projectLedgerRepository = projectLedgerRepository;
        _submissionRepository = submissionRepository;
        _cache = cache;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(SubmitWorkPackageCommand request, CancellationToken cancellationToken)
    {
        // 1. 工作包存在且未停用
        var wp = await _workPackageRepository.GetByIdAsync(request.WorkPackageId, cancellationToken);
        if (wp == null)
            throw new BusinessException("工作包不存在");
        if (wp.Status == ProjectStatus.Disabled)
            throw new BusinessException("工作包已停用，不允许提交");

        // 2. 所属项目存在且未停用
        var project = await _projectRepository.GetByIdAsync(wp.ProjectId, cancellationToken);
        if (project == null)
            throw new BusinessException("所属项目不存在");
        if (project.Status == ProjectStatus.Disabled)
            throw new BusinessException("所属项目已停用，不允许提交汇总");

        // 3. 工作包当月有效台账
        var wpLedger = await _wpLedgerRepository.GetActiveAsync(request.WorkPackageId, request.YearMonth, cancellationToken);
        if (wpLedger == null)
            throw new BusinessException($"工作包在 {request.YearMonth} 没有有效台账，无法提交");

        // 4. 上次提交值（同一工作包同年月最近一次）
        var lastSubmission = await _submissionRepository.GetLatestAsync(request.WorkPackageId, request.YearMonth, cancellationToken);
        var lastCashflow = lastSubmission?.Cashflow ?? 0m;
        var lastFunding = lastSubmission?.Funding ?? 0m;

        // 5. 项目当月台账（不存在则新建）
        var projectLedger = await _projectLedgerRepository.GetActiveAsync(wp.ProjectId, request.YearMonth, cancellationToken);
        if (projectLedger == null)
        {
            projectLedger = new ProjectMonthlyLedger
            {
                ProjectId = wp.ProjectId,
                YearMonth = request.YearMonth,
                Cashflow = wpLedger.Cashflow,
                Funding = wpLedger.Funding,
                Remark = $"由工作包 {wp.Code} 提交生成",
                Status = LedgerStatus.Active
            };
            await _projectLedgerRepository.AddAsync(projectLedger, cancellationToken);
        }
        else
        {
            // 覆盖语义：原值 − 上次提交值 + 本次提交值
            projectLedger.Cashflow = projectLedger.Cashflow - lastCashflow + wpLedger.Cashflow;
            projectLedger.Funding = projectLedger.Funding - lastFunding + wpLedger.Funding;
            await _projectLedgerRepository.UpdateAsync(projectLedger, cancellationToken);
        }

        // 6. 提交留痕
        var submission = new WorkPackageSubmission
        {
            WorkPackageId = request.WorkPackageId,
            ProjectId = wp.ProjectId,
            YearMonth = request.YearMonth,
            Cashflow = wpLedger.Cashflow,
            Funding = wpLedger.Funding,
            SubmittedAt = DateTime.UtcNow,
            SubmittedBy = _currentUser.UserName ?? "system",
            Remark = request.Remark?.Trim()
        };
        await _submissionRepository.AddAsync(submission, cancellationToken);

        // 7. 清项目报表缓存（项目台账变了）
        await _cache.IncreaseVersionAsync(CacheKeys.ReportProjectVersion, cancellationToken);

        return true;
    }
}