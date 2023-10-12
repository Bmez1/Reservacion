using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.QueryFiltros;

namespace ReservaHotel.Domain.Ports
{
    public interface IHabitacionRepository
    {
        Task<IEnumerable<Habitacion>> GetHabitacionesAsync();

        Task<IEnumerable<Habitacion>> GetHabitacionesByFiltroAsync(FiltroBusquedaHabitacion filtro);

        Task<Habitacion> GetHabitacionByIdAsync(Guid habitacionId);
    }
}
