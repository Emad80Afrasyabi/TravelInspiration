using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TravelInspiration.API.Shared.Domain;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.DomainEvents;

namespace TravelInspiration.API.Shared.Persistence;

public sealed class TravelInspirationDbContext(DbContextOptions<TravelInspirationDbContext> options, IPublisher publisher) : DbContext(options)
{
    public DbSet<Itinerary> Itineraries => Set<Itinerary>();
    public DbSet<Stop> Stops => Set<Stop>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SeedData(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TravelInspirationDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Itinerary>().HasData(
            new Itinerary("A Trip to Paris", "dummyuserid")
            {
                Id = 1,
                Description = "Five great days in Paris",
                CreatedBy = "DATASEED",
                CreatedOn = new DateTime(2025, 8, 6, 0, 0, 0, DateTimeKind.Utc)
            },
            new Itinerary("Antwerp Extravaganza", "dummyuserid")
            {
                Id = 2,
                Description = "A week in beautiful Antwerp",
                CreatedBy = "DATASEED",
                CreatedOn = new DateTime(2025, 8, 6, 0, 0, 0, DateTimeKind.Utc)
            });

        modelBuilder.Entity<Stop>().HasData(
            new Stop("The Eiffel Tower")
            {
                Id = 1,
                ItineraryId = 1,
                ImageUri = new Uri("http://localhost:5158/images/eiffeltower.jpg"),
                CreatedBy = "DATASEED",
                CreatedOn = new DateTime(2025, 8, 6, 0, 0, 0, DateTimeKind.Utc)
            },
            new Stop("The Louvre")
            {
                Id = 2,
                ItineraryId = 1,
                ImageUri = new Uri("http://localhost:5158/images/louvre.jpg"),
                CreatedBy = "DATASEED",
                CreatedOn = new DateTime(2025, 8, 6, 0, 0, 0, DateTimeKind.Utc)
            },
            new Stop("Père Lachaise Cemetery")
            {
                Id = 3,
                ItineraryId = 1,
                ImageUri = new Uri("http://localhost:5158/images/perelachaise.jpg"),
                CreatedBy = "DATASEED",
                CreatedOn = new DateTime(2025, 8, 6, 0, 0, 0, DateTimeKind.Utc)
            },
            new Stop("The Royal Museum of Beautiful Arts")
            {
                Id = 4,
                ItineraryId = 2,
                ImageUri = new Uri("http://localhost:5158/images/royalmuseum.jpg"),
                CreatedBy = "DATASEED",
                CreatedOn = new DateTime(2025, 8, 6, 0, 0, 0, DateTimeKind.Utc)
            },
            new Stop("Saint Paul's Church")
            {
                Id = 5,
                ItineraryId = 2,
                ImageUri = new Uri("http://localhost:5158/images/stpauls.jpg"),
                CreatedBy = "DATASEED",
                CreatedOn = new DateTime(2025, 8, 6, 0, 0, 0, DateTimeKind.Utc)
            },
            new Stop("Michelin Restaurant Visit")
            {
                Id = 6,
                ItineraryId = 2,
                ImageUri = new Uri("http://localhost:5158/images/michelin.jpg"),
                CreatedBy = "DATASEED",
                CreatedOn = new DateTime(2025, 8, 6, 0, 0, 0, DateTimeKind.Utc)
            });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "TODO";
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = "TODO";
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = "TODO";
                    break;
            }
        }
        
        DomainEvent[] domainEvents = ChangeTracker.Entries<IHasDomainEvent>()
                                                  .Select(entityEntry => entityEntry.Entity.DomainEvents)
                                                  .SelectMany(events => events)
                                                  .Where(domainEvent => !domainEvent.IsPublished)
                                                  .ToArray();

        foreach (DomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
            domainEvent.IsPublished = true;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
