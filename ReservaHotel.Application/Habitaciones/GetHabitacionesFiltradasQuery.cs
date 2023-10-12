using MediatR;
using ReservaHotel.Domain.Dto;

namespace ReservaHotel.Application.Habitaciones;

public record GetHabitacionesFiltradasQuery(
    DateTime fechaEntrada, DateTime fechaSalida, Guid? tipoHabitacion
    ) : IRequest<IEnumerable<HabitacionDto>>;
