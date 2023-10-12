using ReservaHotel.Domain.Entities;

namespace ReservaHotel.Api.Tests
{
    public class HabitacionTestBuilder
    {
        private int _numeroHabitacion = 101;
        private int _capacidadPersonas = 2;
        private int _numeroCamas = 1;
        private double _precioNoche = 100.0;
        private TipoHabitacion _tipoHabitacion = TipoHabitacion.TipoHabitacionConId(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        private EstadoHabitacion _estadoHabitacion = EstadoHabitacion.EstadoHabitacionConId(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        private Guid _id = Guid.Parse("11111111-1111-1111-1111-111111111111");

        public Habitacion Build()
        {
            return new Habitacion(
                numeroHabitacion: _numeroHabitacion,
                capacidadPersonas: _capacidadPersonas,
                numeroCamas: _numeroCamas,
                precioNoche: _precioNoche,
                tipoHabitacion: _tipoHabitacion,
                estadoHabitacion: _estadoHabitacion
            )
            {
                Id = _id
            };
        }

        public HabitacionTestBuilder WithNumeroHabitacion(int numeroHabitacion)
        {
            _numeroHabitacion = numeroHabitacion;
            return this;
        }

        public HabitacionTestBuilder WithCapacidadPersonas(int capacidadPersonas)
        {
            _capacidadPersonas = capacidadPersonas;
            return this;
        }

        public HabitacionTestBuilder WithNumeroCamas(int numeroCamas)
        {
            _numeroCamas = numeroCamas;
            return this;
        }

        public HabitacionTestBuilder WithPrecioNoche(double precioNoche)
        {
            _precioNoche = precioNoche;
            return this;
        }

        public HabitacionTestBuilder WithTipoHabitacionId(TipoHabitacion tipoHabitacion)
        {
            _tipoHabitacion = tipoHabitacion;
            return this;
        }

        public HabitacionTestBuilder WithEstadoHabitacionId(EstadoHabitacion estadoHabitacion)
        {
            _estadoHabitacion = estadoHabitacion;
            return this;
        }

        public HabitacionTestBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }
    }
}
