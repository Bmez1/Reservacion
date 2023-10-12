using Microsoft.EntityFrameworkCore;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Exceptions;
using ReservaHotel.Infrastructure.DataSource;
using ReservaHotel.Infrastructure.Ports;
using System.Linq.Expressions;

namespace ReservaHotel.Infrastructure.Adapters;

public class GenericRepository<T> : BaseRepositoryGeneric<T> where T : DomainEntity
{
    public GenericRepository(DataContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<T>> GetManyAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeStringProperties = "", bool isTracking = false)
    {
        IQueryable<T> query = Context.Set<T>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeStringProperties))
        {
            foreach (var includeProperty in includeStringProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync().ConfigureAwait(false);
        }

        return (!isTracking) ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();
    }

    public override async Task<T> AddAsync(T entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity), "Entity can not be null");
        await Dataset.AddAsync(entity);
        return entity;
    }

    public override void DeleteEntity(T entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity), "Entity can not be null");
        Dataset.Remove(entity);
    }

    public override async Task<T> GetOneAsync(Guid id)
    {
        return await Dataset.FindAsync(id) ?? throw new EntityNotFoundException(typeof(T).Name, id.ToString());
    }

    public override void UpdateEntity(T entity)
    {
        Dataset.Update(entity);
    }
}
