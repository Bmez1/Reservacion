using MediatR;
using ReservaHotel.Domain.Dtos;
using ReservaHotel.Domain.Dtos.Inputs;
using ReservaHotel.Domain.Services;

namespace ReservaHotel.Application.Reservaciones;

public class CreateReservacionCommandHandler : IRequestHandler<CreateReservacionCommand, ReservacionCreadaDto>
{
    private readonly CreateReservacionService _service;

    public CreateReservacionCommandHandler(CreateReservacionService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async Task<ReservacionCreadaDto> Handle(CreateReservacionCommand request, CancellationToken cancellationToken)
    {
        var reservacion = await _service.RegistrarReservacionAsync(
            new CreateReservacionDto(request!.FechaEntrada, request!.FechaSalida,
            request!.ClienteId, request!.HabitacionId, request.Comentario), cancellationToken
        );

        return new ReservacionCreadaDto
        {
            ReservacionId = reservacion.Id,
            CantidadDias = reservacion.CantidadDias,
            CodigoReservacion = reservacion.CodigoReservacion,
            Comentario = reservacion.Comentario,
            FechaEntrada = reservacion.FechaEntrada.ToShortDateString(),
            FechaSalida = reservacion.FechaSalida.ToShortDateString(),
            PrecioTotal = reservacion.PrecioTotal
        };
    }
}
