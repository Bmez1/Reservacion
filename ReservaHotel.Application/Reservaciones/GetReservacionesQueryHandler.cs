using MediatR;
using ReservaHotel.Domain.Dtos;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Ports;

namespace ReservaHotel.Application.Reservaciones
{
    public class GetReservacionesQueryHandler : IRequestHandler<GetReservacionesQuery, IEnumerable<ReservacionDatosDto>>
    {
        private readonly IReservacionRepository _reservacionRepository;

        public GetReservacionesQueryHandler(IReservacionRepository reservacionRepository)
            => _reservacionRepository = reservacionRepository;

        public async Task<IEnumerable<ReservacionDatosDto>> Handle(GetReservacionesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Reservacion> reservaciones = await _reservacionRepository.GetReservacionesAsync(request!.estadoReservacionId);
            return reservaciones.Select(reservacion =>
            {
                return new ReservacionDatosDto
                {
                    ReservacionId = reservacion.Id,
                    CantidadDias = reservacion.CantidadDias,
                    CodigoReservacion = reservacion.CodigoReservacion,
                    Comentario = reservacion.Comentario,
                    EstadoReservacion = reservacion.EstadoReservacion.Estado,
                    FechaEntrada = reservacion.FechaEntrada.ToShortDateString(),
                    FechaSalida = reservacion.FechaSalida.ToShortDateString(),
                    NombreCliente = reservacion.Cliente.ObtenerNombreCompleto(),
                    NumeroHabitacion = reservacion.Habitacion.NumeroHabitacion,
                    PrecioTotal = reservacion.PrecioTotal
                };
            });
        }
    }
}
