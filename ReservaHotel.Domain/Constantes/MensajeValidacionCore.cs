namespace ReservaHotel.Domain.Constantes
{
    public record MensajeValidacionCore
    {
        public static string FechaEntradaMenorActual { get; } = "La fecha de entrada no puede ser menor a la fecha actual.";
        public static string FechaSalidaMenorActual { get; } = "La fecha de salida debe ser mayor a la fecha actual.";
        public static string FechaSalidaMenorEntrada { get; } = "La fecha de salida debe ser mayor a la fecha de entrada.";
        public static string ClienteConMasDe2Reservaciones { get; } = "Error: Un cliente tiene permitido reservar un máximo de 3 habitaciones";
        public static string PropiedadNula(string nombrePropiedad) => $"{nombrePropiedad} no puede ser nula o estar vacía";
    }
}
