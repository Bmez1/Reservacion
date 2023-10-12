using MediatR;
using ReservaHotel.Application.Habitaciones;
using ReservaHotel.Domain.Dto;

namespace ReservaHotel.Api.ApiHandlers.EndPoints
{
    public static class HabitacionApi
    {
        public static RouteGroupBuilder MapHabitacion(this IEndpointRouteBuilder routeHandler)
        {
            routeHandler.MapGet("/{id}", async (IMediator mediator, Guid id) =>
            {
                return Results.Ok(await mediator.Send(new GetHabitacionQuery(id)));
            })
            .Produces(StatusCodes.Status200OK, typeof(HabitacionDto));

            routeHandler.MapGet("/", async (IMediator mediator) =>
            {
                return Results.Ok(await mediator.Send(new GetHabitacionesQuery()));
            })
            .Produces(StatusCodes.Status200OK, typeof(IEnumerable<HabitacionDto>));

            routeHandler.MapGet("/filtro", async (IMediator mediator, DateTime fechaEntrada, DateTime fechaSalida, Guid? tipoHabitacion) =>
            {
                GetHabitacionesFiltradasQuery filtroBusquedaHabitacion = new GetHabitacionesFiltradasQuery(fechaEntrada, fechaSalida, tipoHabitacion);
                return Results.Ok(await mediator.Send(filtroBusquedaHabitacion));
            })
            .Produces(StatusCodes.Status200OK, typeof(IEnumerable<HabitacionDto>));

            return (RouteGroupBuilder)routeHandler;
        }
    }
}
