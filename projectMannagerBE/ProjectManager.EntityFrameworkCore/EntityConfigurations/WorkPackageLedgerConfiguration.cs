using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;

namespace ProjectManager.EntityFrameworkCore.Configurations;

public class WorkPackageLedgerConfiguration : IEntityTypeConfiguration<WorkPackageMonthlyLedger>
{
    public void Configure(EntityTypeBuilder<WorkPackageMonthlyLedger> builder)
    {
        builder.ToTable("WorkPackageMonthlyLedgers");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.YearMonth).IsRequired();
        builder.Property(x => x.Cashflow).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Funding).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Remark).HasMaxLength(200);
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();

        builder.HasIndex(x => new { x.WorkPackageId, x.YearMonth })
            .IsUnique()
            .HasFilter("`Status` = 1");

        builder.HasOne<WorkPackage>()
            .WithMany()
            .HasForeignKey(x => x.WorkPackageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).HasMaxLength(50);
        builder.Property(x => x.UpdatedBy).HasMaxLength(50);
    }
}