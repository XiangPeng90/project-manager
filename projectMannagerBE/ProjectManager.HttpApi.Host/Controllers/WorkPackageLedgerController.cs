using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Contracts.Commands.Ledgers;
using ProjectManager.Application.Contracts.Dtos.Ledgers;

namespace ProjectManager.HttpApi.Host.Controllers;

[ApiController]
[Route("api/workpackages/{workPackageId}/ledgers")]
[Authorize]
public class WorkPackageLedgerController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkPackageLedgerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public Task<List<WorkPackageLedgerDto>> GetList(long workPackageId, [FromQuery] int? from, [FromQuery] int? to)
        => _mediator.Send(new GetWorkPackageLedgersQuery { WorkPackageId = workPackageId, From = from, To = to });

    [HttpPost]
    public Task<long> Create(long workPackageId, [FromBody] CreateWorkPackageLedgerCommand command)
    {
        command.WorkPackageId = workPackageId;
        return _mediator.Send(command);
    }

    [HttpPut("{id}")]
    public Task<bool> Update(long workPackageId, long id, [FromBody] UpdateWorkPackageLedgerCommand command)
    {
        command.Id = id;
        return _mediator.Send(command);
    }

    [HttpPatch("{id}/void")]
    public Task<bool> Void(long workPackageId, long id)
        => _mediator.Send(new VoidWorkPackageLedgerCommand { Id = id });
}