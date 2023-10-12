using MediatR;
using ReservaHotel.Domain.Dto;

namespace ReservaHotel.Application.Habitaciones
{
    public record GetHabitacionesQuery : IRequest<IEnumerable<HabitacionDto>>;
}
