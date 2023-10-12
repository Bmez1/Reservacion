namespace ReservaHotel.Domain.Dtos
{
    public class ReservacionCanceladaDto
    {
        public double PrecioNocheHabotacion { get; init; }
        public double PrecioAPagar { get; init; }
        public string CodigoReservacion { get; init; }
        public string Comentario { get; init; }
        public string Mensaje { get; init; }

        public ReservacionCanceladaDto(string codigoReservacion, double precioAPagar, string comentario, double precioNocheHabotacion)
        {
            PrecioAPagar = precioAPagar;
            Comentario = comentario;
            CodigoReservacion = codigoReservacion;
            Mensaje = "La reservacion ha sido cancelada con exito";
            PrecioNocheHabotacion = precioNocheHabotacion;
        }
    }
}
