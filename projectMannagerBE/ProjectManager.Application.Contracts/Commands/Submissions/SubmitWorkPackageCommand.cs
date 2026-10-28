using MediatR;

namespace ProjectManager.Application.Contracts.Commands.Submissions;

public class SubmitWorkPackageCommand : IRequest<bool>
{
    public long WorkPackageId { get; set; }
    public int YearMonth { get; set; }
    public string? Remark { get; set; }
}