using ReservaHotel.Domain.Entities;

namespace ReservaHotel.Domain.Tests
{
    public class ReservacionTestBuilder
    {
        private Guid _id = Guid.NewGuid();
        private DateTime _fechaEntrada = DateTime.Now;
        private DateTime _fechaSalida = DateTime.Now.AddDays(3);
        private Cliente _cliente = Cliente.ClienteConId(Guid.NewGuid());
        private string _comentario = string.Empty;
        private Habitacion _habitacion = Habitacion.CrearHabitacionConId(Guid.NewGuid());
        private EstadoReservacion _estadoReservacion = EstadoReservacion
            .CrearEstadoReservacionConId(Guid.NewGuid());

        public Reservacion Build()
        {
            return new Reservacion(
                fechaEntrada: _fechaEntrada,
                fechaSalida: _fechaSalida,
                cliente: _cliente,
                habitacion: _habitacion,
                estadoReservacion: _estadoReservacion
                )
            {
                Id = _id,
                Comentario = _comentario
            };
        }

        public ReservacionTestBuilder WithComentario(string comentario)
        {
            _comentario = comentario;
            return this;
        }

        public ReservacionTestBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public ReservacionTestBuilder WithFechaEntrada(DateTime fechaEntrada)
        {
            _fechaEntrada = fechaEntrada;
            return this;
        }

        public ReservacionTestBuilder WithFechaSalida(DateTime fechaSalida)
        {
            _fechaSalida = fechaSalida;
            return this;
        }

        public ReservacionTestBuilder WithCliente(Cliente cliente)
        {
            _cliente = cliente;
            return this;
        }

        public ReservacionTestBuilder WithHabitacion(Habitacion habitacion)
        {
            _habitacion = habitacion;
            return this;
        }

        public ReservacionTestBuilder WithEstadoReservacion(EstadoReservacion estadoReservacion)
        {
            _estadoReservacion = estadoReservacion;
            return this;
        }
    }
}
