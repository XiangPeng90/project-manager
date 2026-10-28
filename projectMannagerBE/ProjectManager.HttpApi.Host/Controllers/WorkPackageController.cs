using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Contracts.Commands.WorkPackages;
using ProjectManager.Application.Contracts.Dtos;
using ProjectManager.Application.Contracts.Dtos.WorkPackages;

namespace ProjectManager.HttpApi.Host.Controllers;

[ApiController]
[Route("api/workpackages")]
[Authorize]
public class WorkPackageController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkPackageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public Task<long> Create([FromBody] CreateWorkPackageCommand command)
        => _mediator.Send(command);

    [HttpPut("{id}")]
    public Task<bool> Update(long id, [FromBody] UpdateWorkPackageCommand command)
    {
        command.Id = id;
        return _mediator.Send(command);
    }

    [HttpPatch("{id}/disable")]
    public Task<bool> Disable(long id)
        => _mediator.Send(new DisableWorkPackageCommand { Id = id });
    
    [HttpGet]
    public Task<PagedResult<WorkPackageDto>> GetList([FromQuery] GetWorkPackageListQuery query)
        => _mediator.Send(query);

    [HttpGet("{id}")]
    public Task<WorkPackageDetailDto> GetDetail(long id)
        => _mediator.Send(new GetWorkPackageDetailQuery { Id = id });
}