using DirectoryService.Infrastructure.Departments;
using DirectoryService.Infrastructure.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.EntityConfigurations;

public class DepartmentPositionConfiguration: IEntityTypeConfiguration<DepartmentPosition>
{
    public void Configure(EntityTypeBuilder<DepartmentPosition> builder)
    {
        builder.ToTable("department_position");

        builder.HasKey(dp => dp.Id).HasName("pk_department_position");

        builder.Property(dp => dp.Id).HasColumnName("id");
        
        builder.Property(dp => dp.DepartmentId)
            .IsRequired()
            .HasColumnName("department_id");
        
        builder.Property(dp => dp.PositionId)
            .IsRequired()
            .HasColumnName("position_id");
        
        builder.HasOne<Department>()
            .WithMany(d => d.DepartmentPositions)
            .HasForeignKey(dl => dl.DepartmentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<Position>()
            .WithMany(p => p.DepartmentPositions)
            .HasForeignKey(dp => dp.PositionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(dp => new { dp.DepartmentId, dp.PositionId })
            .IsUnique()
            .HasDatabaseName("ux_department_position");
    }
}