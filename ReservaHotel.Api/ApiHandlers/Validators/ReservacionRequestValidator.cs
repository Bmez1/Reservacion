using FluentValidation;
using ReservaHotel.Application.Reservaciones;
using ReservaHotel.Domain.Constantes;

namespace ReservaHotel.Api.ApiHandlers.Validators;

public class ReservacionRequestValidator : AbstractValidator<CreateReservacionCommand>
{
    public ReservacionRequestValidator()
    {
        RuleFor(reservacion => reservacion.FechaEntrada)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Now.Date)
            .WithMessage(MensajeValidacionCore.FechaEntradaMenorActual);
        RuleFor(reservacion => reservacion.FechaSalida)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Now.Date)
            .WithMessage(MensajeValidacionCore.FechaSalidaMenorActual)
            .GreaterThan(filtro => filtro.FechaEntrada)
            .WithMessage(MensajeValidacionCore.FechaSalidaMenorEntrada);
        RuleFor(reservacion => reservacion.HabitacionId).NotEmpty();
        RuleFor(reservacion => reservacion.ClienteId).NotEmpty();
    }
}
