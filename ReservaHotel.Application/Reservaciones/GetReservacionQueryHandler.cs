using MediatR;
using ReservaHotel.Domain.Dtos;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Ports;

namespace ReservaHotel.Application.Reservaciones
{
    public class GetReservacionQueryHandler : IRequestHandler<GetReservacionQuery, ReservacionDatosDto>
    {
        private readonly IReservacionRepository _reservacionRepository;

        public GetReservacionQueryHandler(IReservacionRepository reservacionRepository)
            => _reservacionRepository = reservacionRepository;

        public async Task<ReservacionDatosDto> Handle(GetReservacionQuery request, CancellationToken cancellationToken)
        {
            Reservacion reservacion = await _reservacionRepository.GetReservacionByIdAsync(request!.reservacionId);
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
        }
    }
}
