using NSubstitute;
using ReservaHotel.Domain.Constantes;
using ReservaHotel.Domain.Dtos.Inputs;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Exceptions;
using ReservaHotel.Domain.Ports;
using ReservaHotel.Domain.Services;

namespace ReservaHotel.Domain.Tests;

public class ReservacionTests
{
    private readonly CreateReservacionService _service;
    private readonly IReservacionRepository _reservacionRepository;
    private readonly IHabitacionRepository _habitacionRepository;
    private readonly IEstadoReservacionRepository _estadoReservacionRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReservacionTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _reservacionRepository = Substitute.For<IReservacionRepository>();
        _habitacionRepository = Substitute.For<IHabitacionRepository>();
        _estadoReservacionRepository = Substitute.For<IEstadoReservacionRepository>();
        _clienteRepository = Substitute.For<IClienteRepository>();
        _habitacionRepository = Substitute.For<IHabitacionRepository>();

        _service = new CreateReservacionService(_reservacionRepository, _habitacionRepository,
            _clienteRepository, _estadoReservacionRepository, _unitOfWork);
    }

    public static IEnumerable<object[]> ReservacionesTestFallido => new List<object[]>
        {
            new object[]
            {
                new ReservacionTestBuilder().WithFechaEntrada(DateTime.Now.AddDays(-1)),
                MensajeValidacionCore.FechaEntradaMenorActual
            },
            new object[]
            {
                new ReservacionTestBuilder()
                    .WithFechaEntrada(DateTime.Now.AddDays(5))
                    .WithFechaSalida (DateTime.Now.AddDays(2)),
                MensajeValidacionCore.FechaSalidaMenorEntrada
            },
            new object[]
            {
                new ReservacionTestBuilder()
                    .WithFechaEntrada(DateTime.Now.AddDays(5).Date)
                    .WithFechaSalida (DateTime.Now.AddDays(5).Date),
                MensajeValidacionCore.FechaSalidaMenorEntrada
            },
            new object[]
            {
                new ReservacionTestBuilder()
                    .WithFechaEntrada(DateTime.Now.Date)
                    .WithFechaSalida (DateTime.Now.Date),
                MensajeValidacionCore.FechaSalidaMenorActual
            },
            new object[]
            {
                new ReservacionTestBuilder()
                .WithCliente(null),
                MensajeValidacionCore.PropiedadNula("Cliente")
            },
            new object[]
            {
                new ReservacionTestBuilder()
                .WithEstadoReservacion(null),
                MensajeValidacionCore.PropiedadNula("EstadoReservacion")
            },
            new object[]
            {
                new ReservacionTestBuilder()
                .WithHabitacion(null),
                MensajeValidacionCore.PropiedadNula("Habitacion")
            },
        };

    /// <summary>
    /// Objeto que contiene los casos de prueba para calcular el precio de una reservacion
    /// al cancelarse. (reservacion, fecha de cancelación, precio esperado)
    /// </summary>
    public static IEnumerable<object[]> DatosReservacionACancelar => new List<object[]>
    {
        new object[]
        {
            new ReservacionTestBuilder()
            .WithFechaEntrada(DateTime.Now.AddDays(2))
            .WithFechaSalida(DateTime.Now.AddDays(3))
            .WithHabitacion(Habitacion.CrearHabitacionConPrecio(10_000))
            .WithComentario("No aplica el 15% adicional")
            .Build(),
            DateTime.Now,
            10_000 // Se cobra precio de noche de la habitación
        },
        new object[]
        {
            new ReservacionTestBuilder()
            .WithFechaEntrada(DateTime.Now)
            .WithFechaSalida(DateTime.Now.AddDays(1))
            .WithHabitacion(Habitacion.CrearHabitacionConPrecio(10_000))
            .WithComentario("Aplica el 15% adicional")
            .Build(),
            DateTime.Now,
            11_500 // Se cobra el precio de la habitación + un 15% al cancelar con menos de 24 horas de la entrada
        },

    };

    public static IEnumerable<object[]> PrecioNocheTest => new List<object[]>
        {
            new object[] {1, 1_000, 1_000}, // cantidad Dias, precioNoche, PrecioTotal
            new object[] {2, 5_000, 10_000},
            new object[] {7, 8_000, 56_000},
            new object[] {8, 7_000, 50_400},
            new object[] {20, 1_000, 18_000},
        };

    [Fact]
    public void CrearReservacionTestExitoso()
    {
        DateTime fechaEntrada = DateTime.Now;
        DateTime fechaSalida = DateTime.Now.AddDays(2);
        Cliente cliente = Cliente.ClienteConId(Guid.NewGuid());
        Habitacion habitacion = Habitacion.CrearHabitacionConId(Guid.NewGuid());
        EstadoReservacion estadoReservacion = EstadoReservacion.CrearEstadoReservacionConId(Guid.NewGuid());

        var reservacion = new Reservacion(fechaEntrada, fechaSalida, cliente, habitacion, estadoReservacion);

        Assert.NotEqual(Guid.Empty, reservacion.Id);
    }

    [Theory]
    [MemberData(nameof(ReservacionesTestFallido))]
    public void CrearReservacionTestFallido(ReservacionTestBuilder reservacion, string response)
    {
        var error = Record.Exception(() => reservacion.Build());

        Assert.IsType<CoreBusinessException>(error);
        Assert.Equal(response, error.Message);
    }

    [Theory]
    [MemberData(nameof(PrecioNocheTest))]
    public void VerificarPrecioReservacion(int cantidadDias, double valorNoche, double valorEsperado)
    {
        Reservacion reservacion = new ReservacionTestBuilder()
            .WithFechaEntrada(DateTime.Now.Date)
            .WithFechaSalida(DateTime.Now.AddDays(cantidadDias).Date)
            .WithHabitacion(Habitacion.CrearHabitacionConPrecio(valorNoche))
            .Build();
        reservacion.CalcularPrecioReservacion();


        Assert.Equal(valorEsperado, reservacion.PrecioTotal);
    }

    [Theory]
    [MemberData(nameof(DatosReservacionACancelar))]
    public void VerificarPrecioReservacionAlCancelar(Reservacion reservacion, DateTime fechaCancelacion, double valorEsperado)
    {
        reservacion.CalcularPrecioReservacion(fechaCancelacion);

        Assert.Equal(valorEsperado, reservacion.PrecioTotal);
    }

    [Fact]
    public async Task ClienteMasDe2ReservacionesActivas()
    {
        Guid clienteId = Guid.NewGuid();
        string mensajeError = MensajeValidacionCore.ClienteConMasDe2Reservaciones;

        Reservacion reservacion = new ReservacionTestBuilder()
            .WithCliente(Cliente.ClienteConId(clienteId))
            .Build();

        _reservacionRepository.GetCantidadReservasActivasByPersonaAsync(clienteId).Returns(3);

        var error = await Record.ExceptionAsync(() => _service.RegistrarReservacionAsync(new CreateReservacionDto(reservacion.FechaEntrada,
            reservacion.FechaSalida, reservacion.Cliente.Id, reservacion.Habitacion.Id, reservacion.Comentario))
        );

        await _reservacionRepository.Received().GetCantidadReservasActivasByPersonaAsync(Arg.Any<Guid>());

        Assert.IsType<CoreBusinessException>(error);
        Assert.Equal(mensajeError, error.Message);
    }
}
