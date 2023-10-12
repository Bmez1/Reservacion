using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Exceptions;
using ReservaHotel.Domain.Ports;
using ReservaHotel.Infrastructure.Ports;

namespace ReservaHotel.Infrastructure.Adapters
{
    [Repository]
    public class ClienteRepository : IClienteRepository
    {
        private readonly BaseRepositoryGeneric<Cliente> _dataSource;

        public ClienteRepository(BaseRepositoryGeneric<Cliente> dataSource)
        {
            _dataSource = dataSource;

        }
        public async Task<Cliente> GetClienteByIdAsync(Guid clienteId)
        {
            return await _dataSource.GetOneAsync(clienteId);
        }
    }
}
