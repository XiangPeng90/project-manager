using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Contracts.Commands.Submissions;

namespace ProjectManager.HttpApi.Host.Controllers;

[ApiController]
[Route("api/workpackages")]
[Authorize]
public class SubmissionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubmissionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{workPackageId}/submit")]
    public Task<bool> Submit(long workPackageId, [FromBody] SubmitWorkPackageCommand command)
    {
        command.WorkPackageId = workPackageId;
        return _mediator.Send(command);
    }
}