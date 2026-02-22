using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.EntityConfigurations;

public class DepartmentLocationConfiguration: IEntityTypeConfiguration<DepartmentLocation>
{
    public void Configure(EntityTypeBuilder<DepartmentLocation> builder)
    {
        builder.ToTable("department_location");

        builder.HasKey(dl => dl.Id).HasName("pk_department_location");

        builder.Property(dl => dl.Id).HasColumnName("id");
        
        builder.Property(dl => dl.DepartmentId)
            .IsRequired()
            .HasColumnName("department_id");
        
        builder.Property(dl => dl.LocationId)
            .IsRequired()
            .HasColumnName("location_id");
        
        builder.HasOne<Department>()
            .WithMany(d => d.DepartmentLocations)
            .HasForeignKey(dl => dl.DepartmentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<Location>()
            .WithMany(l => l.DepartmentLocations)
            .HasForeignKey(dl => dl.LocationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(dl => new { dl.DepartmentId, dl.LocationId })
            .IsUnique()
            .HasDatabaseName("ux_department_location");
    }
}