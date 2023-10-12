using ReservaHotel.Application.Reservaciones;

namespace ReservaHotel.Api.Tests
{
    public class ReservacionCommandTestBuilder
    {
        private DateTime _fecha_Entrada = DateTime.Now;
        private DateTime _fecha_Salida = DateTime.Now.AddDays(1);
        private string _comentario = string.Empty;
        private Guid _clienteId = Guid.NewGuid();
        private Guid _habitacionId = Guid.NewGuid();

        public CreateReservacionCommand Build()
        {
            return new CreateReservacionCommand
            {
                FechaEntrada = _fecha_Entrada,
                FechaSalida = _fecha_Salida,
                Comentario = _comentario,
                ClienteId = _clienteId,
                HabitacionId = _habitacionId
            };
        }

        public ReservacionCommandTestBuilder WithFechaEntrada(DateTime fecha_Entrada)
        {
            _fecha_Entrada = fecha_Entrada;
            return this;
        }

        public ReservacionCommandTestBuilder WithFechaSalida(DateTime fecha_Salida)
        {
            _fecha_Salida = fecha_Salida;
            return this;
        }

        public ReservacionCommandTestBuilder WithComentario(string comentario)
        {
            _comentario = comentario;
            return this;
        }

        public ReservacionCommandTestBuilder WithClienteId(Guid clienteId)
        {
            _clienteId = clienteId;
            return this;
        }

        public ReservacionCommandTestBuilder WithHabitacionId(Guid habitacionId)
        {
            _habitacionId = habitacionId;
            return this;
        }
    }
}
