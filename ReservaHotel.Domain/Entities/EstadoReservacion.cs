namespace ReservaHotel.Domain.Entities
{
    public class EstadoReservacion : DomainEntity
    {
        public string Estado { get; init; }
        public string Comentario { get; init; }
        public ICollection<Reservacion> Reservaciones { get; } = new List<Reservacion>();

        public EstadoReservacion(string estado)
        {
            Estado = estado;
            Comentario = string.Empty;
        }

        public static EstadoReservacion CrearEstadoReservacionConId(Guid id)
        {
            return new EstadoReservacion(string.Empty)
            {
                Id = id
            };
        }
    }
}
