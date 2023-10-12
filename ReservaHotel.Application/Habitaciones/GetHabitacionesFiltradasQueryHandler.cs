using MediatR;
using ReservaHotel.Domain.Dto;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Ports;
using ReservaHotel.Domain.QueryFiltros;

namespace ReservaHotel.Application.Habitaciones;

public class GetHabitacionesFiltradasQueryHandler : IRequestHandler<GetHabitacionesFiltradasQuery, IEnumerable<HabitacionDto>>
{
    private readonly IHabitacionRepository _repository;

    public GetHabitacionesFiltradasQueryHandler(IHabitacionRepository repository) => _repository = repository;

    public async Task<IEnumerable<HabitacionDto>> Handle(GetHabitacionesFiltradasQuery request, CancellationToken cancellationToken)
    {
        FiltroBusquedaHabitacion filtro = new FiltroBusquedaHabitacion
        (
            fechaEntrada: request!.fechaEntrada,
            fechaSalida: request!.fechaSalida,
            tipoHabitacion: request!.tipoHabitacion
        );

        IEnumerable<Habitacion> habitaciones = await _repository.GetHabitacionesByFiltroAsync(filtro);
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
