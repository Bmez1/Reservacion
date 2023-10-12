using Microsoft.EntityFrameworkCore;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Infrastructure.DataSource;
using System.Linq.Expressions;

namespace ReservaHotel.Infrastructure.Ports;

public abstract class BaseRepositoryGeneric<T> where T : DomainEntity
{
    private readonly DataContext _context;
    private readonly DbSet<T> _dataset;

    public DataContext Context { get => _context; }
    public DbSet<T> Dataset { get => _dataset; }

    protected BaseRepositoryGeneric(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dataset = Context.Set<T>();
    }

    public abstract Task<T> GetOneAsync(Guid id);

    public abstract Task<IEnumerable<T>> GetManyAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeStringProperties = "",
        bool isTracking = false);

    public abstract Task<T> AddAsync(T entity);

    public abstract void UpdateEntity(T entity);

    public abstract void DeleteEntity(T entity);
}
