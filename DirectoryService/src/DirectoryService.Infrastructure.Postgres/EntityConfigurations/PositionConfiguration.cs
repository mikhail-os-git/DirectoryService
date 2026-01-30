using DirectoryService.Domain.Positions;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configurations;

public class PositionConfiguration: IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("positions");
        builder.HasKey(p => p.Id).HasName("pk_position");
        
        builder.Property(p => p.Id).HasColumnName("id");

        builder.Property(p => p.PositionName)
            .HasConversion(pn => pn.Value, name => PositionName.FromDb(name))
            .IsRequired()
            .HasMaxLength(PositionName.MAX_LENGTH)
            .HasColumnName("name");

        builder.HasIndex(p => p.PositionName)
            .IsUnique()
            .HasDatabaseName("ux_position_name");

        builder.Property(p => p.Description)
            .IsRequired(false)
            .HasMaxLength(PositionName.MAX_LENGTH)
            .HasColumnName("description");

        builder.Property(p => p.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
        
        builder.Property(p => p.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");
    }
}