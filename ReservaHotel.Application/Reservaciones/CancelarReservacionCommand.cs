using MediatR;
using ReservaHotel.Domain.Dtos;

namespace ReservaHotel.Application.Reservaciones
{
    public record CancelarReservacionCommand(Guid reservacionId) : IRequest<ReservacionCanceladaDto>;
}
