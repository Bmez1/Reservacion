using MediatR;
using ReservaHotel.Domain.Dtos;

namespace ReservaHotel.Application.Reservaciones
{
    public record GetReservacionQuery(Guid reservacionId) : IRequest<ReservacionDatosDto>;
}
