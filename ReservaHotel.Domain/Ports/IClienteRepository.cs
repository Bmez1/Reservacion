using ReservaHotel.Domain.Entities;

namespace ReservaHotel.Domain.Ports
{
    public interface IClienteRepository
    {
        Task<Cliente> GetClienteByIdAsync(Guid clienteId);
    }
}
