using MediatR;
using ReservaHotel.Domain.Dto;

namespace ReservaHotel.Application.Habitaciones
{
    public record GetHabitacionQuery(Guid habitacionId) : IRequest<HabitacionDto>;
}
