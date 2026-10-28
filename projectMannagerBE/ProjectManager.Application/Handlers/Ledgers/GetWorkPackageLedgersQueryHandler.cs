using MediatR;
using ProjectManager.Application.Contracts.Dtos.Ledgers;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.Ledgers;

public class GetWorkPackageLedgersQueryHandler : IRequestHandler<GetWorkPackageLedgersQuery, List<WorkPackageLedgerDto>>
{
    private readonly IWorkPackageLedgerRepository _ledgerRepository;
    private readonly IWorkPackageRepository _workPackageRepository;

    public GetWorkPackageLedgersQueryHandler(
        IWorkPackageLedgerRepository ledgerRepository,
        IWorkPackageRepository workPackageRepository)
    {
        _ledgerRepository = ledgerRepository;
        _workPackageRepository = workPackageRepository;
    }

    public async Task<List<WorkPackageLedgerDto>> Handle(GetWorkPackageLedgersQuery request, CancellationToken cancellationToken)
    {
        var wp = await _workPackageRepository.GetByIdAsync(request.WorkPackageId, cancellationToken);
        if (wp == null)
            throw new BusinessException("工作包不存在");

        var ledgers = await _ledgerRepository.GetByWorkPackageAsync(request.WorkPackageId, request.From, request.To, cancellationToken);

        return ledgers.Select(l => new WorkPackageLedgerDto
        {
            Id = l.Id,
            WorkPackageId = l.WorkPackageId,
            YearMonth = l.YearMonth,
            Cashflow = l.Cashflow,
            Funding = l.Funding,
            Remark = l.Remark,
            Status = (int)l.Status,
            CreatedAt = l.CreatedAt
        }).ToList();
    }
}