namespace ReservaHotel.Domain.Configuration;

public record InformacionEstadoEntidad()
{
    public Guid EstadoDisponibleHabitacionId { get; init; }
    public Guid EstadoActivoReservacionId { get; init; }
    public Guid EstadoCanceladoReservacionId { get; init; }
};
