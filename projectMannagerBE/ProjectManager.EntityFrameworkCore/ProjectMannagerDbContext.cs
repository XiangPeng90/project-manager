using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectManager.Domain.Services;
using ProjectManager.Domain.Shared;

namespace ProjectManager.EntityFrameworkCore;

public class ProjectManagerDbContext : DbContext
{
    public ProjectManagerDbContext(DbContextOptions<ProjectManagerDbContext> options)
        : base(options) { }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<WorkPackage> WorkPackages => Set<WorkPackage>();
    public DbSet<ProjectMonthlyLedger> ProjectMonthlyLedgers => Set<ProjectMonthlyLedger>();
    public DbSet<WorkPackageMonthlyLedger> WorkPackageMonthlyLedgers => Set<WorkPackageMonthlyLedger>();
    public DbSet<WorkPackageSubmission> WorkPackageSubmissions => Set<WorkPackageSubmission>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // 自动应用当前程序集下所有 IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectManagerDbContext).Assembly);
    }
}

/// <summary>
/// 自动填写审计字段
/// </summary>
public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUser;

    public AuditSaveChangesInterceptor(ICurrentUserService currentUser)
    {
        _currentUser = currentUser;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAudit(DbContext? context)
    {
        if (context == null) return;

        var now = DateTime.UtcNow;
        var user = _currentUser.UserName ?? "system";

        foreach (var entry in context.ChangeTracker.Entries<AuditedEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = user;
                entry.Entity.IsDeleted = false;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = user;
            }
        }
    }
}