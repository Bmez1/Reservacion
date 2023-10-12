using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReservaHotel.Domain.Configuration;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Exceptions;
using ReservaHotel.Domain.Ports;
using ReservaHotel.Infrastructure.Ports;

namespace ReservaHotel.Infrastructure.Adapters
{
    [Repository]
    public class ReservacionRepository : IReservacionRepository
    {
        private readonly BaseRepositoryGeneric<Reservacion> _dataSource;
        private readonly InformacionEstadoEntidad _estadoActivoReservacion;

        public ReservacionRepository(BaseRepositoryGeneric<Reservacion> dataSource, IOptions<InformacionEstadoEntidad> confi)
        {
            _dataSource = dataSource;
            _estadoActivoReservacion = confi?.Value
                ?? throw new ArgumentNullException(nameof(confi));
        }

        public async Task<Reservacion> CrearReservacionAsync(Reservacion reservacion)
        {
            return await _dataSource.AddAsync(reservacion);
        }

        public async Task<int> GetCantidadReservasActivasByPersonaAsync(Guid personaId)
        {
            int cantidad = await _dataSource.Dataset
                .CountAsync(r => r.Cliente.Id == personaId &&
                r.EstadoReservacion.Id == _estadoActivoReservacion.EstadoActivoReservacionId);
            return cantidad;
        }

        public async Task<Reservacion> GetReservacionByIdAsync(Guid reservacionId)
        {
            var reservacion = await _dataSource.Dataset
                .Include(r => r.Cliente)
                .Include(r => r.EstadoReservacion)
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(r => r.Id == reservacionId);
            return reservacion ?? throw new EntityNotFoundException(nameof(Reservacion), reservacionId.ToString());
        }

        public async Task<Reservacion> GetReservacionActivaByIdAsync(Guid reservacionId)
        {
            var reservacion = await _dataSource.Dataset
                .Include (r => r.Habitacion)
                .FirstOrDefaultAsync(r => r.Id == reservacionId &&
                r.EstadoReservacion.Id == _estadoActivoReservacion.EstadoActivoReservacionId);

            return reservacion ?? throw new EntityNotFoundException(nameof(Reservacion), reservacionId.ToString());
        }

        public async Task<IEnumerable<Reservacion>> GetReservacionesAsync(Guid? estadoReservacionId)
        {
            var reservaciones = _dataSource.Dataset
                .Include(r => r.Cliente)
                .Include(r => r.EstadoReservacion)
                .Include(r => r.Habitacion)
                .AsNoTracking();

            if (estadoReservacionId is not null)
            {
                reservaciones = reservaciones.Where(reservacion
                    => reservacion.EstadoReservacion.Id == estadoReservacionId);
            }

            return await reservaciones.ToListAsync();
        }

        public void CancelarReservacion(Reservacion reservacion)
        {
            _dataSource.UpdateEntity(reservacion);
        }
    }
}
