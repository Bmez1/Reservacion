using MediatR;
using ReservaHotel.Domain.Dto;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Ports;

namespace ReservaHotel.Application.Habitaciones
{
    public class GetHabitacionQueryHandler : IRequestHandler<GetHabitacionQuery, HabitacionDto>
    {
        private readonly IHabitacionRepository _repository;

        public GetHabitacionQueryHandler(IHabitacionRepository repository) => _repository = repository;

        public async Task<HabitacionDto> Handle(GetHabitacionQuery request, CancellationToken cancellationToken)
        {
            Habitacion habitacion = await _repository.GetHabitacionByIdAsync(request!.habitacionId);
            return new HabitacionDto
            {
                Id = habitacion.Id,
                NumeroHabitacion = habitacion.NumeroHabitacion,
                CapacidadPersonas = habitacion.CapacidadPersonas,
                NumeroCamas = habitacion.NumeroCamas,
                PrecioNoche = habitacion.PrecioNoche,
                Descripcion = habitacion.Descripcion,
                Tipo = habitacion.TipoHabitacion.Tipo,
                Estado = habitacion.EstadoHabitacion.Estado
            };
        }
    }
}
