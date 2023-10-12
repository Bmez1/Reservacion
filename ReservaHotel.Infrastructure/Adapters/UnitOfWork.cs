using Microsoft.EntityFrameworkCore;
using ReservaHotel.Domain.Ports;
using ReservaHotel.Infrastructure.DataSource;

namespace ReservaHotel.Infrastructure.Adapters;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(CancellationToken? cancellationToken = null)
    {
        var token = cancellationToken ?? new CancellationTokenSource().Token;

        _context.ChangeTracker.DetectChanges();

        var entryStatus = new Dictionary<EntityState, string> {
            {EntityState.Added, "CreatedOn"},
            {EntityState.Modified, "LastModifiedOn"}
        };

        foreach (var entry in _context.ChangeTracker.Entries())
        {
            if (entryStatus.TryGetValue(entry.State, out var propertyName))
            {
                entry.Property(propertyName).CurrentValue = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync(token);
    }
}
