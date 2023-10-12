namespace ReservaHotel.Domain.Dtos
{
    public record ReservacionDatosDto
    {
        public Guid ReservacionId { get; set; }
        public string FechaEntrada { get; init; } = string.Empty;
        public string FechaSalida { get; init; } = string.Empty;
        public double PrecioTotal { get; init; }
        public string Comentario { get; init; } = string.Empty;
        public int CantidadDias { get; init; }
        public string CodigoReservacion { get; init; } = string.Empty;
        public string NombreCliente { get; init; } = string.Empty;
        public int NumeroHabitacion { get; init; }
        public string EstadoReservacion { get; init; } = string.Empty;
    }
}
