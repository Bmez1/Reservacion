namespace ReservaHotel.Domain.Entities
{
    public class Cliente : DomainEntity
    {
        public string Identificacion { get; init; }
        public string Nombres { get; init; }
        public string Apellidos { get; init; }
        public string Correo { get; init; }
        public string Telefono { get; init; }

        public ICollection<Reservacion> Reservaciones { get; } = new List<Reservacion>();

        public Cliente(string identificacion, string nombres, string apellidos, string correo, string telefono)
        {
            Identificacion = identificacion;
            Nombres = nombres;
            Apellidos = apellidos;
            Correo = correo;
            Telefono = telefono;
        }

        public string ObtenerNombreCompleto()
        {
            return Nombres + " " + Apellidos;
        }

        public static Cliente ClienteConId(Guid clienteId)
        {
            return new Cliente("xxxx", "xxxx", "xxxx", "xxxx", "xxxx")
            {
                Id = clienteId
            };
        }
    }
}
