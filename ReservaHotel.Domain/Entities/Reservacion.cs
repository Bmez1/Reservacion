using ReservaHotel.Domain.Constantes;
using ReservaHotel.Domain.Exceptions;
using System.Globalization;

namespace ReservaHotel.Domain.Entities
{
    public class Reservacion : DomainEntity
    {
        private const int HORA_TARIFA_ADICIONAL = 24;
        private const double PORCENTAJEDESCUENTO = .1;
        private const double PORCENTAJE_TARIFA_ADICIONAL = .15;
        private const int CANTIDAD_DIAS_PARA_DESCUENTO = 8;
        private const int CIENPORCIENTO = 100;


        private DateTime _fechaEntrada;
        private DateTime _fechaSalida;
        private Cliente _cliente = null!;
        private Habitacion _habitacion = null!;
        private EstadoReservacion _estadoReservacion = null!;

#pragma warning disable CS8618 // Constructor para serialización
        private Reservacion() { }
#pragma warning restore CS8618 // Constructor para serialización

        public Reservacion(DateTime fechaEntrada, DateTime fechaSalida,
            Cliente cliente, Habitacion habitacion, EstadoReservacion estadoReservacion)
        {
            FechaEntrada = fechaEntrada;
            FechaSalida = fechaSalida;
            Cliente = cliente;
            Habitacion = habitacion;
            CantidadDias = CalcularCantidadDias();
            CodigoReservacion = GenerarCodigoReservacion();
            EstadoReservacion = estadoReservacion;
            Comentario = string.Empty;
            Id = Guid.NewGuid();
            CalcularPrecioReservacion();
        }

        public DateTime FechaEntrada
        {
            get => _fechaEntrada;
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

        public Cliente Cliente 
        { 
            get => _cliente;
            init
            {
                if (value == null)
                {
                    throw new CoreBusinessException(MensajeValidacionCore.PropiedadNula(nameof(Cliente)));
                }
                _cliente = value;
            }
        }

        public Habitacion Habitacion 
        { 
            get => _habitacion;
            init
            {
                if (value == null)
                {
                    throw new CoreBusinessException(MensajeValidacionCore.PropiedadNula(nameof(Habitacion)));
                }
                _habitacion = value;
            }
        }
        public EstadoReservacion EstadoReservacion
        {
            get => _estadoReservacion;
            set
            {
                if (value == null)
                {
                    throw new CoreBusinessException(MensajeValidacionCore.PropiedadNula(nameof(EstadoReservacion)));
                }
                _estadoReservacion = value; 
            }
        }

        public double PrecioTotal { get; private set; }
        public string Comentario { get; set; }
        public int CantidadDias { get; private init; }
        public string CodigoReservacion { get; private init; }

        private int CalcularCantidadDias()
        {
            int dias = (FechaSalida - FechaEntrada).Days;
            return dias;
        }

        private static string GenerarCodigoReservacion()
        {
            CultureInfo cultura = CultureInfo.InvariantCulture;
            string fechaFormatoString = DateTime.Now.ToString("yyyyMMddHHmmss", cultura);
            int numeroAletorio = new Random().Next(1000, 9999);

            return fechaFormatoString + numeroAletorio.ToString(cultura);
        }

        public void CalcularPrecioReservacion(DateTime? fechaCancelacion = null)
        {
            double precioHabitacion = Habitacion.PrecioNoche;

            if (fechaCancelacion is null)
            {
                double descuento = CantidadDias >= CANTIDAD_DIAS_PARA_DESCUENTO ? (CantidadDias * precioHabitacion) * PORCENTAJEDESCUENTO : 0;
                PrecioTotal = (CantidadDias * precioHabitacion) - descuento;
                if (descuento > 0)
                {
                    Comentario += $" - se aplica descuento del {PORCENTAJEDESCUENTO * CIENPORCIENTO}% por reservación mayor a 7 días.";
                }
            }
            else
            {
                PrecioTotal = CalcularPrecioAlCancelar(precioHabitacion, (DateTime)fechaCancelacion);
            }
        }

        private double CalcularPrecioAlCancelar(double precioHabitacion, DateTime fechaCancelacion)
        {
            double horas = (FechaEntrada - fechaCancelacion).TotalHours;
            if (horas < HORA_TARIFA_ADICIONAL)
            {
                Comentario += $" - se cobra precio de la habitación más recargo del {PORCENTAJE_TARIFA_ADICIONAL * CIENPORCIENTO}% " +
                    $"por cancelación con menos de 24 horas de anticipación.";
                return (precioHabitacion * (1 + PORCENTAJE_TARIFA_ADICIONAL));
            }
            return precioHabitacion;
        }
    }
}
