using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Repositories;

public interface ISubmissionRepository : IBaseRepository<WorkPackageSubmission>
{
    Task<WorkPackageSubmission?> GetLatestAsync(long workPackageId, int yearMonth, CancellationToken ct = default);
    Task<List<WorkPackageSubmission>> GetByWorkPackageAsync(long workPackageId, CancellationToken ct = default);
}