namespace ReservaHotel.Domain.Ports;

public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken? cancellationToken = null);
}
