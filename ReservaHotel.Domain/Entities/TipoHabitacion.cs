namespace ReservaHotel.Domain.Entities
{
    public class TipoHabitacion : DomainEntity
    {
        public string Tipo { get; init; }
        public string Comentario { get; init; }

        public ICollection<Habitacion> Habitaciones { get; } = new List<Habitacion>();

        public TipoHabitacion(string tipo)
        {
            Tipo = tipo;
            Comentario = string.Empty;
            Id = Guid.NewGuid();
        }

        public static TipoHabitacion TipoHabitacionPorDefecto()
        {
            return new TipoHabitacion("Individual");
        }

        public static TipoHabitacion TipoHabitacionConId(Guid id)
        {
            return new TipoHabitacion("Individual")
            {
                Id = id
            };
        }
    }
}
