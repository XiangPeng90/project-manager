using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Contracts.Commands.Projects;
using ProjectManager.Application.Contracts.Dtos.Projects;
using ProjectManager.Application.Contracts.Dtos;

namespace ProjectManager.HttpApi.Host.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public Task<long> Create([FromBody] CreateProjectCommand command)
        => _mediator.Send(command);

    [HttpPut("{id}")]
    public Task<bool> Update(long id, [FromBody] UpdateProjectCommand command)
    {
        command.Id = id;
        return _mediator.Send(command);
    }

    [HttpPatch("{id}/disable")]
    public Task<bool> Disable(long id)
        => _mediator.Send(new DisableProjectCommand { Id = id });

    [HttpGet]
    public Task<PagedResult<ProjectDto>> GetList([FromQuery] GetProjectListQuery query)
        => _mediator.Send(query);

    [HttpGet("{id}")]
    public Task<ProjectDetailDto> GetDetail(long id)
        => _mediator.Send(new GetProjectDetailQuery { Id = id });
}