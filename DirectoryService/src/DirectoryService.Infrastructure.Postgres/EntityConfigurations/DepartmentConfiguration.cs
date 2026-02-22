using DirectoryService.Domain.Departments;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Path = DirectoryService.Domain.ValueObjects.Path;

namespace DirectoryService.Infrastructure.EntityConfigurations;

public class DepartmentConfiguration: IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");

        builder.HasKey(d => d.Id).HasName("pk_departments");
        
        builder.Property(d => d.Id)
            .HasColumnName("id");

        builder.Property(d => d.DepartmentName)
            .HasConversion(dn => dn.Value, name => DepartmentName.FromDb(name))
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(DepartmentName.MAX_LENGTH);

        builder.Property(d => d.Identifier)
            .HasConversion(i => i.Value, identidier => Identifier.FromDb(identidier))
            .HasColumnName("identifier")
            .IsRequired()
            .HasMaxLength(DepartmentName.MAX_LENGTH);

        builder.Property(d => d.ParentId)
            .IsRequired(false)
            .HasColumnName("parent_id");

        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(d => d.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(d => d.Path)
            .HasConversion(p => p.Value, path => Path.FromDb(path))
            .IsRequired()
            .HasColumnName("path");

        builder.Property(d => d.Depth)
            .IsRequired()
            .HasColumnName("depth");
        
        builder.Property(d => d.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        
        builder.Property(d => d.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
        
        builder.Property(d => d.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

    }
}