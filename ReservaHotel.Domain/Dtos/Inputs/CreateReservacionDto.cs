namespace ReservaHotel.Domain.Dtos.Inputs
{
    public record CreateReservacionDto(DateTime fechaEntrada, DateTime fechaSalida,
            Guid clienteId, Guid habitacionId, string comentario);
}
