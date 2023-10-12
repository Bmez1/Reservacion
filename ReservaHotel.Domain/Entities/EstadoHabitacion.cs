namespace ReservaHotel.Domain.Entities
{
    public class EstadoHabitacion : DomainEntity
    {
        public string Estado { get; init; }
        public string Comentario { get; init; }

        public ICollection<Habitacion> Habitaciones { get; } = new List<Habitacion>();

        public EstadoHabitacion(string estado)
        {
            Estado = estado;
            Comentario = string.Empty;
            Id = Guid.NewGuid();
        }

        public static EstadoHabitacion EstadoHabitacionPorDefecto()
        {
            return new EstadoHabitacion("Activo");
        }

        public static EstadoHabitacion EstadoHabitacionConId(Guid id)
        {
            return new EstadoHabitacion("Activo")
            {
                Id = id
            };
        }
    }
}
