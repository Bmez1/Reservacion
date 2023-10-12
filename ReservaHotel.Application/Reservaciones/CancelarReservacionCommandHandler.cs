using MediatR;
using ReservaHotel.Domain.Dtos;
using ReservaHotel.Domain.Services;

namespace ReservaHotel.Application.Reservaciones;

public class CancelarReservacionCommandHandler : IRequestHandler<CancelarReservacionCommand, ReservacionCanceladaDto>
{
    private readonly CancelarReservacionService _service;

    public CancelarReservacionCommandHandler(CancelarReservacionService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async Task<ReservacionCanceladaDto> Handle(CancelarReservacionCommand request, CancellationToken cancellationToken)
    {
        var reservacion = await _service.CancelarReservacionAsync(request!.reservacionId);
        return new ReservacionCanceladaDto(reservacion.CodigoReservacion, reservacion.PrecioTotal, reservacion.Comentario, reservacion.Habitacion.PrecioNoche);
    }
}
