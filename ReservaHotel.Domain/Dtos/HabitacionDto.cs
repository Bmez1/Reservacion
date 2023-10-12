namespace ReservaHotel.Domain.Dto;

public record HabitacionDto
{
    public Guid Id { get; init; }
    public int NumeroHabitacion { get; init; }
    public int CapacidadPersonas { get; init; }
    public int NumeroCamas { get; init; }
    public double PrecioNoche { get; init; }
    public string Descripcion { get; init; } = string.Empty;
    public string Tipo { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
}
