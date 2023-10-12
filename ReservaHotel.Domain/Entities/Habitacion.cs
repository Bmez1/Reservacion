namespace ReservaHotel.Domain.Entities
{
    public class Habitacion : DomainEntity
    {
        public int NumeroHabitacion { get; init; }
        public int CapacidadPersonas { get; init; }
        public int NumeroCamas { get; init; }
        public double PrecioNoche { get; init; }
        public string Descripcion { get; init; }
        public TipoHabitacion TipoHabitacion { get; }
        public EstadoHabitacion EstadoHabitacion { get; }
        public ICollection<Reservacion> Reservaciones { get; } = new List<Reservacion>();

#pragma warning disable CS8618 // Constructor para serialización
        private Habitacion() { }
#pragma warning restore CS8618 // Constructor para serialización

        public Habitacion(int numeroHabitacion, int capacidadPersonas, int numeroCamas, 
            double precioNoche, TipoHabitacion tipoHabitacion, EstadoHabitacion estadoHabitacion)
        {
            NumeroHabitacion = numeroHabitacion;
            CapacidadPersonas = capacidadPersonas;
            NumeroCamas = numeroCamas;
            PrecioNoche = precioNoche;
            Descripcion = string.Empty;
            TipoHabitacion = tipoHabitacion;
            EstadoHabitacion = estadoHabitacion;
        }

        public static Habitacion CrearHabitacionConId(Guid habitacionId)
        {
            return new Habitacion(1, 1, 1, 1,
                TipoHabitacion.TipoHabitacionPorDefecto(), EstadoHabitacion.EstadoHabitacionPorDefecto())
            {
                Id = habitacionId,
            };
        }

        public static Habitacion CrearHabitacionConPrecio(double precioNoche)
        {
            return new Habitacion(1, 1, 1, precioNoche,
                TipoHabitacion.TipoHabitacionPorDefecto(), EstadoHabitacion.EstadoHabitacionPorDefecto());
        }
    }
}
