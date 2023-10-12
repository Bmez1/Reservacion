using MediatR;
using ReservaHotel.Domain.Dtos;

namespace ReservaHotel.Application.Reservaciones
{
    public record CreateReservacionCommand : IRequest<ReservacionCreadaDto>
    {
        public DateTime FechaEntrada { get; init; }
        public DateTime FechaSalida { get; init; }
        public string Comentario { get; set; } = string.Empty;
        public Guid ClienteId { get; init; }
        public Guid HabitacionId { get; init; }
    };
}
