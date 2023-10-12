using ReservaHotel.Domain.Constantes;
using ReservaHotel.Domain.Exceptions;

namespace ReservaHotel.Domain.QueryFiltros
{
    public class FiltroBusquedaHabitacion
    {
        private DateTime _fechaEntrada;
        private DateTime _fechaSalida;

        public FiltroBusquedaHabitacion(DateTime fechaEntrada, DateTime fechaSalida, Guid? tipoHabitacion)
        {
            FechaEntrada = fechaEntrada;
            FechaSalida = fechaSalida;
            TipoHabitacion = tipoHabitacion;
        }

        public DateTime FechaEntrada
        {
            get { return _fechaEntrada; }
            init
            {
                if (value < DateTime.Now.Date)
                {
                    throw new CoreBusinessException(MensajeValidacionCore.FechaEntradaMenorActual);
                }
                _fechaEntrada = value;
            }
        }

        public DateTime FechaSalida
        {
            get { return _fechaSalida; }
            init
            {
                if (value <= DateTime.Now.Date)
                {
                    throw new CoreBusinessException(MensajeValidacionCore.FechaSalidaMenorActual);
                }
                if (value <= _fechaEntrada)
                {
                    throw new CoreBusinessException(MensajeValidacionCore.FechaSalidaMenorEntrada);
                }
                _fechaSalida = value;
            }
        }

        public Guid? TipoHabitacion { get; set; }
    }
}
