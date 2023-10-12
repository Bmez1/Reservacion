using ReservaHotel.Domain.Entities;

namespace ReservaHotel.Domain.Ports
{
    public interface IEstadoReservacionRepository
    {
        Task<EstadoReservacion> GetEstadoReservacionActivoAsync();
        Task<EstadoReservacion> GetEstadoReservacionCanceladaAsync();
    }
}
