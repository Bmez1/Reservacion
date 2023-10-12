using ReservaHotel.Domain.Entities;

namespace ReservaHotel.Domain.Ports
{
    public interface IReservacionRepository
    {
        Task<Reservacion> GetReservacionByIdAsync(Guid reservacionId);

        Task<Reservacion> GetReservacionActivaByIdAsync(Guid reservacionId);

        Task<IEnumerable<Reservacion>> GetReservacionesAsync(Guid? estadoReservacionId);

        Task<Reservacion> CrearReservacionAsync(Reservacion reservacion);

        Task<int> GetCantidadReservasActivasByPersonaAsync(Guid personaId);

        void CancelarReservacion(Reservacion reservacion);

    }
}
