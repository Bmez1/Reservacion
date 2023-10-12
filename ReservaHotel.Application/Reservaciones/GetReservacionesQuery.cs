using MediatR;
using ReservaHotel.Domain.Dtos;

namespace ReservaHotel.Application.Reservaciones
{
    public record GetReservacionesQuery(Guid? estadoReservacionId) : IRequest<IEnumerable<ReservacionDatosDto>>;
}
