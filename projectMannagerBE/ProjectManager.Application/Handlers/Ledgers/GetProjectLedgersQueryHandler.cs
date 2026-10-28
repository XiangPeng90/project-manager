using MediatR;
using ProjectManager.Application.Contracts.Dtos.Ledgers;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.Ledgers;

public class GetProjectLedgersQueryHandler : IRequestHandler<GetProjectLedgersQuery, List<ProjectLedgerDto>>
{
    private readonly IProjectLedgerRepository _ledgerRepository;
    private readonly IProjectRepository _projectRepository;

    public GetProjectLedgersQueryHandler(
        IProjectLedgerRepository ledgerRepository,
        IProjectRepository projectRepository)
    {
        _ledgerRepository = ledgerRepository;
        _projectRepository = projectRepository;
    }

    public async Task<List<ProjectLedgerDto>> Handle(GetProjectLedgersQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null)
            throw new BusinessException("项目不存在");

        var ledgers = await _ledgerRepository.GetByProjectAsync(request.ProjectId, request.From, request.To, cancellationToken);

        return ledgers.Select(l => new ProjectLedgerDto
        {
            Id = l.Id,
            ProjectId = l.ProjectId,
            YearMonth = l.YearMonth,
            Cashflow = l.Cashflow,
            Funding = l.Funding,
            Remark = l.Remark,
            Status = (int)l.Status,
            CreatedAt = l.CreatedAt
        }).ToList();
    }
}