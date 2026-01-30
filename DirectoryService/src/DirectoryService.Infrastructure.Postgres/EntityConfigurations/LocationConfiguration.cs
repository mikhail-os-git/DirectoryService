using DirectoryService.Domain.Common.Constants;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configurations;

public class LocationConfiguration: IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");

        builder.HasKey(l => l.Id).HasName("pk_loction");

        builder.Property(l => l.Id).HasColumnName("id");

        builder.Property(l => l.LocationName)
            .HasConversion(ln => ln.Value, name => LocationName.FromDb(name))
            .IsRequired()
            .HasMaxLength(LocationName.MAX_LENGTH)
            .HasColumnName("name");

        builder.HasIndex(l => l.LocationName)
            .IsUnique()
            .HasDatabaseName("ux_locations_name");

        builder.OwnsOne(l => l.Address, lb =>
        {
            lb.ToJson("address").HasColumnType("jsonb");

            lb.Property(a => a.Country).HasJsonPropertyName("country");
            lb.Property(a => a.City).HasJsonPropertyName("city");
            lb.Property(a => a.Street).HasJsonPropertyName("street");
            lb.Property(a => a.HouseNumber).HasJsonPropertyName("house_number");
            lb.Property(a => a.PostalCode).HasJsonPropertyName("postal_code");

        });

        builder.Navigation(l => l.Address).IsRequired();
        builder.Property(l => l.Timezone)
            .HasConversion(tz => tz.Value, timezone => Timezone.FromDb(timezone))
            .IsRequired()
            .HasColumnName("timezone");
            
        builder.Property(l => l.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnName("is_active");

        builder.Property(l => l.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
        
        builder.Property(l => l.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

    }
}