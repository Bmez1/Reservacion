using ReservaHotel.Domain.QueryFiltros;

namespace ReservaHotel.Domain.Tests
{
    public class HabitacionesFiltradasQueryTestBuilder
    {
        private DateTime _fechaEntrada;
        private DateTime _fechaSalida;
        private Guid? _tipoHabitacion = Guid.Empty;

        public HabitacionesFiltradasQueryTestBuilder WithFechaEntrada(DateTime fecha)
        {
            _fechaEntrada = fecha;
            return this;
        }

        public HabitacionesFiltradasQueryTestBuilder WithFechaSalida(DateTime fecha)
        {
            _fechaSalida = fecha;
            return this;
        }

        public HabitacionesFiltradasQueryTestBuilder WithTipoHabitacion(Guid tipo)
        {
            _tipoHabitacion = tipo;
            return this;
        }

        public FiltroBusquedaHabitacion Build()
        {
            return new FiltroBusquedaHabitacion(_fechaEntrada, _fechaSalida, _tipoHabitacion);
        }
    }

}
