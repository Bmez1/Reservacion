using MediatR;
using ReservaHotel.Domain.Dto;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Ports;

namespace ReservaHotel.Application.Habitaciones
{
    public class GetHabitacionesQueryHandler : IRequestHandler<GetHabitacionesQuery, IEnumerable<HabitacionDto>>
    {
        private readonly IHabitacionRepository _repository;

        public GetHabitacionesQueryHandler(IHabitacionRepository repository) => _repository = repository;

        public async Task<IEnumerable<HabitacionDto>> Handle(GetHabitacionesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Habitacion> habitaciones = await _repository.GetHabitacionesAsync();
            return habitaciones.Select(habitacion => new HabitacionDto
            {
                Id = habitacion.Id,
                NumeroHabitacion = habitacion.NumeroHabitacion,
                CapacidadPersonas = habitacion.CapacidadPersonas,
                NumeroCamas = habitacion.NumeroCamas,
                PrecioNoche = habitacion.PrecioNoche,
                Descripcion = habitacion.Descripcion,
                Tipo = habitacion.TipoHabitacion.Tipo,
                Estado = habitacion.EstadoHabitacion.Estado
            });
        }
    }
}
