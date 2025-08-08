using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelInspiration.API.Shared.Domain.Entities;

namespace TravelInspiration.API.Shared.Persistence.Configurations;

public sealed class StopConfiguration : IEntityTypeConfiguration<Stop>
{
    public void Configure(EntityTypeBuilder<Stop> builder)
    {
        builder.ToTable("Stops");
        builder.Property(stop => stop.Id).UseIdentityColumn();
        builder.Property(stop => stop.Name).IsRequired().HasMaxLength(200);
        builder.HasOne(stop => stop.Itinerary)
               .WithMany(itinerary => itinerary.Stops)
               .HasForeignKey(stop => stop.ItineraryId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.Ignore(stop => stop.DomainEvents);
        builder.Property(stop => stop.Suggested).HasDefaultValue(false);
    }
}
