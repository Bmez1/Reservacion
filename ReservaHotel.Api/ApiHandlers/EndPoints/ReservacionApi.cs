using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservaHotel.Api.Filters;
using ReservaHotel.Application.Reservaciones;
using ReservaHotel.Domain.Dtos;

namespace ReservaHotel.Api.ApiHandlers.EndPoints
{
    public static class ReservacionApi
    {
        public static RouteGroupBuilder MapReservacion(this IEndpointRouteBuilder routeHandler)
        {
            routeHandler.MapGet("/{reservacionId}", async (IMediator mediator, [FromRoute] Guid reservacionId) =>
            {
                return Results.Ok(await mediator.Send(new GetReservacionQuery(reservacionId)));
            })
            .Produces(StatusCodes.Status200OK, typeof(ReservacionDatosDto));

            routeHandler.MapGet("/", async (IMediator mediator, [FromQuery] Guid? estadoReservacionId) =>
            {
                return Results.Ok(await mediator.Send(new GetReservacionesQuery(estadoReservacionId)));
            })
            .Produces(StatusCodes.Status200OK, typeof(IEnumerable<ReservacionDatosDto>));

            // create resource, validating command like api controller
            routeHandler.MapPost("/", async (IMediator mediator, [Validate] CreateReservacionCommand reservacion) =>
            {
                ReservacionCreadaDto reservacionAgregada = await mediator.Send(reservacion);
                return Results.Created(new Uri($"/api/reservacion/{reservacionAgregada.ReservacionId}", UriKind.Relative), reservacionAgregada);
            })
            .Produces(statusCode: StatusCodes.Status201Created);

            routeHandler.MapPatch("/{reservacionId}", async (IMediator mediator, [FromRoute] Guid reservacionId) =>
            {
                return Results.Ok(await mediator.Send(new CancelarReservacionCommand(reservacionId)));
            })
            .Produces(statusCode: StatusCodes.Status200OK);

            return (RouteGroupBuilder)routeHandler;
        }
    }
}
