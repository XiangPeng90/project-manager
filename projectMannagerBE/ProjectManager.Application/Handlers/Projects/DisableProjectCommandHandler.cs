using MediatR;
using ProjectManager.Application.Contracts.Commands.Projects;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Shared.Enums;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.Projects;

public class DisableProjectCommandHandler : IRequestHandler<DisableProjectCommand, bool>
{
    private readonly IProjectRepository _projectRepository;

    public DisableProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<bool> Handle(DisableProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken);
        if (project == null)
            throw new BusinessException("项目不存在");

        if (project.Status == ProjectStatus.Disabled)
            return true; // 已停用，幂等返回

        project.Status = ProjectStatus.Disabled;
        await _projectRepository.UpdateAsync(project, cancellationToken);
        return true;
    }
}