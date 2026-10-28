using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Shared.Enums;

namespace ProjectManager.EntityFrameworkCore.Configurations;

public class ProjectLedgerConfiguration : IEntityTypeConfiguration<ProjectMonthlyLedger>
{
    public void Configure(EntityTypeBuilder<ProjectMonthlyLedger> builder)
    {
        builder.ToTable("ProjectMonthlyLedgers");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.YearMonth).IsRequired();
        builder.Property(x => x.Cashflow).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Funding).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Remark).HasMaxLength(200);
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();

        // 过滤唯一索引：有效记录才参与唯一约束
        builder.HasIndex(x => new { x.ProjectId, x.YearMonth })
            .IsUnique()
            .HasFilter("`Status` = 1");

        builder.HasOne<Project>()
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).HasMaxLength(50);
        builder.Property(x => x.UpdatedBy).HasMaxLength(50);
    }
}