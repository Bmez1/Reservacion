using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReservaHotel.Domain.Configuration;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Exceptions;
using ReservaHotel.Domain.Ports;
using ReservaHotel.Domain.QueryFiltros;
using ReservaHotel.Infrastructure.Ports;

namespace ReservaHotel.Infrastructure.Adapters
{
    [Repository]
    public class HabitacionRepository : IHabitacionRepository
    {
        private readonly InformacionEstadoEntidad _habitacionDisponibleInformacion;
        private readonly BaseRepositoryGeneric<Habitacion> _dataSource;

        public HabitacionRepository(IOptions<InformacionEstadoEntidad> informacionHabitacion, BaseRepositoryGeneric<Habitacion> dataSource)
        {
            _dataSource = dataSource
                ?? throw new ArgumentNullException(nameof(dataSource));
            _habitacionDisponibleInformacion = informacionHabitacion?.Value
                ?? throw new ArgumentNullException(nameof(informacionHabitacion));
        }

        public async Task<Habitacion> GetHabitacionByIdAsync(Guid habitacionId)
        {
            var response = await _dataSource.Dataset
                .Include(habitacion => habitacion.TipoHabitacion)
                .Include(habitacion => habitacion.EstadoHabitacion)
                .FirstOrDefaultAsync(habitacion => habitacion.Id.Equals(habitacionId));
            return response
                ?? throw new EntityNotFoundException(nameof(Habitacion), habitacionId.ToString());
        }

        public async Task<IEnumerable<Habitacion>> GetHabitacionesAsync()
        {
            return await _dataSource.GetManyAsync(includeStringProperties: "TipoHabitacion,EstadoHabitacion");
        }

        public async Task<IEnumerable<Habitacion>> GetHabitacionesByFiltroAsync(FiltroBusquedaHabitacion filtro)
        {
            var habitacionesDisponibles = _dataSource.Dataset
                .Where(h => !_dataSource.Context.Set<Reservacion>()
                    .Any(r => r.Habitacion.Id == h.Id &&
                               filtro.FechaEntrada < r.FechaSalida &&
                               filtro.FechaSalida > r.FechaEntrada &&
                               r.EstadoReservacion.Id == _habitacionDisponibleInformacion.EstadoActivoReservacionId) &&
                            h.EstadoHabitacion.Id == _habitacionDisponibleInformacion.EstadoDisponibleHabitacionId);

            if (filtro.TipoHabitacion is not null)
            {
                habitacionesDisponibles = habitacionesDisponibles
                    .Where(h => h.TipoHabitacion.Id == filtro.TipoHabitacion);
            }

            // Incluye las entidades TipoHabitacion y EstadoHabitacion una sola vez
            habitacionesDisponibles = habitacionesDisponibles
                .Include(habitacion => habitacion.TipoHabitacion)
                .Include(habitacion => habitacion.EstadoHabitacion);

            return await habitacionesDisponibles.AsNoTracking().ToListAsync();
        }
    }
}
