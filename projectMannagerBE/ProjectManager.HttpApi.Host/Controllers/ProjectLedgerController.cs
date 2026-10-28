using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Contracts.Commands.Ledgers;
using ProjectManager.Application.Contracts.Dtos.Ledgers;

namespace ProjectManager.HttpApi.Host.Controllers;

[ApiController]
[Route("api/projects/{projectId}/ledgers")]
[Authorize]
public class ProjectLedgerController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectLedgerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public Task<List<ProjectLedgerDto>> GetList(long projectId, [FromQuery] int? from, [FromQuery] int? to)
        => _mediator.Send(new GetProjectLedgersQuery { ProjectId = projectId, From = from, To = to });

    [HttpPost]
    public Task<long> Create(long projectId, [FromBody] CreateProjectLedgerCommand command)
    {
        command.ProjectId = projectId;
        return _mediator.Send(command);
    }

    [HttpPut("{id}")]
    public Task<bool> Update(long projectId, long id, [FromBody] UpdateProjectLedgerCommand command)
    {
        command.Id = id;
        return _mediator.Send(command);
    }

    [HttpPatch("{id}/void")]
    public Task<bool> Void(long projectId, long id)
        => _mediator.Send(new VoidProjectLedgerCommand { Id = id });
}