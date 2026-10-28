using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;

namespace ProjectManager.EntityFrameworkCore.Configurations;

public class SubmissionConfiguration : IEntityTypeConfiguration<WorkPackageSubmission>
{
    public void Configure(EntityTypeBuilder<WorkPackageSubmission> builder)
    {
        builder.ToTable("WorkPackageSubmissions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.YearMonth).IsRequired();
        builder.Property(x => x.Cashflow).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Funding).HasColumnType("decimal(18,2)");
        builder.Property(x => x.SubmittedAt).IsRequired();
        builder.Property(x => x.SubmittedBy).HasMaxLength(50);
        builder.Property(x => x.Remark).HasMaxLength(200);

        builder.HasIndex(x => new { x.WorkPackageId, x.YearMonth, x.SubmittedAt });
        builder.HasIndex(x => new { x.ProjectId, x.YearMonth });

        builder.HasOne<WorkPackage>()
            .WithMany()
            .HasForeignKey(x => x.WorkPackageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}