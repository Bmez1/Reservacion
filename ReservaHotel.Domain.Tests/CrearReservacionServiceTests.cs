using NSubstitute;
using ReservaHotel.Domain.Dtos.Inputs;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Ports;
using ReservaHotel.Domain.Services;

namespace ReservaHotel.Domain.Tests;

public class CrearReservacionServiceTests
{
    private readonly CreateReservacionService _service;
    private readonly IReservacionRepository _reservacionRepository;
    private readonly IHabitacionRepository _habitacionRepository;
    private readonly IEstadoReservacionRepository _estadoReservacionRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CrearReservacionServiceTests()
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

    [Fact]
    public async Task CrearReservacionExitosa()
    {
        Guid clienteId = Guid.NewGuid();
        Guid habitacionId = Guid.NewGuid();
        Guid estadoActivoReservacionId = Guid.NewGuid();

        Habitacion habitacion = Habitacion.CrearHabitacionConId(habitacionId);
        Cliente cliente = Cliente.ClienteConId(clienteId);
        EstadoReservacion estadoActivoReservacion = EstadoReservacion
            .CrearEstadoReservacionConId(estadoActivoReservacionId);

        Reservacion reservacion = new ReservacionTestBuilder()
            .WithCliente(Cliente.ClienteConId(clienteId))
            .WithHabitacion(Habitacion.CrearHabitacionConId(habitacionId))
            .Build();

        _reservacionRepository.GetCantidadReservasActivasByPersonaAsync(clienteId).Returns(0);
        _habitacionRepository.GetHabitacionByIdAsync(habitacionId).Returns(habitacion);
        _clienteRepository.GetClienteByIdAsync(clienteId).Returns(cliente);
        _estadoReservacionRepository.GetEstadoReservacionActivoAsync().Returns(estadoActivoReservacion);

        _reservacionRepository.CrearReservacionAsync(Arg.Any<Reservacion>()).Returns(reservacion);
        _unitOfWork.SaveAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        Reservacion result = await _service.RegistrarReservacionAsync(new CreateReservacionDto(reservacion.FechaEntrada,
                reservacion.FechaSalida, reservacion.Cliente.Id, reservacion.Habitacion.Id, reservacion.Comentario));


        await _reservacionRepository.Received().GetCantidadReservasActivasByPersonaAsync(clienteId);
        await _habitacionRepository.Received().GetHabitacionByIdAsync(habitacionId);
        await _clienteRepository.Received().GetClienteByIdAsync(clienteId);
        await _estadoReservacionRepository.Received().GetEstadoReservacionActivoAsync();
        await _reservacionRepository.Received().CrearReservacionAsync(Arg.Any<Reservacion>());
        await _unitOfWork.Received().SaveAsync(Arg.Any<CancellationToken>());

        Assert.Equal(reservacion, result);
    }
}
