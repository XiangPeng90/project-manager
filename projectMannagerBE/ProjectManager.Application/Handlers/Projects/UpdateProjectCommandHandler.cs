using MediatR;
using ProjectManager.Application.Contracts.Commands.Projects;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Shared.Enums;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.Projects;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
{
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken);
        if (project == null)
            throw new BusinessException("项目不存在");

        if (project.Status == ProjectStatus.Disabled)
            throw new BusinessException("项目已停用，不允许修改");

        var exists = await _projectRepository.CodeExistsAsync(request.Code, request.Id, cancellationToken);
        if (exists)
            throw new BusinessException($"项目编号 {request.Code} 已存在");

        project.Code = request.Code.Trim();
        project.Name = request.Name.Trim();
        project.Manager = request.Manager?.Trim();
        project.StartDate = request.StartDate;
        project.EndDate = request.EndDate;
        project.Description = request.Description?.Trim();

        await _projectRepository.UpdateAsync(project, cancellationToken);
        return true;
    }
}