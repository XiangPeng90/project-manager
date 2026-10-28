using MediatR;
using ProjectManager.Application.Contracts.Commands.Projects;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Shared.Enums;
using ProjectManager.Domain.Shared.Exceptions;

namespace ProjectManager.Application.Handlers.Projects;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, long>
{
    private readonly IProjectRepository _projectRepository;

    public CreateProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<long> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        // 业务校验：编号唯一
        var exists = await _projectRepository.CodeExistsAsync(request.Code, null, cancellationToken);
        if (exists)
            throw new BusinessException($"项目编号 {request.Code} 已存在");

        var project = new Project
        {
            Code = request.Code.Trim(),
            Name = request.Name.Trim(),
            Manager = request.Manager?.Trim(),
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Description = request.Description?.Trim(),
            Status = ProjectStatus.Enabled
        };

        await _projectRepository.AddAsync(project, cancellationToken);
        return project.Id;
    }
}