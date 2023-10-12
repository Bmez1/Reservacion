using NSubstitute;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Ports;
using ReservaHotel.Domain.Services;

namespace ReservaHotel.Domain.Tests;

public class CancelarReservacionServiceTests
{
    private readonly CancelarReservacionService _service;
    private readonly IReservacionRepository _reservacionRepository;
    private readonly IEstadoReservacionRepository _estadoReservacionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelarReservacionServiceTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _reservacionRepository = Substitute.For<IReservacionRepository>();
        _estadoReservacionRepository = Substitute.For<IEstadoReservacionRepository>();

        _service = new CancelarReservacionService(_reservacionRepository, _estadoReservacionRepository, _unitOfWork);
    }

    [Fact]
    public async Task CancelarReservacionExitosa()
    {
        Guid reservacionId = Guid.NewGuid();
        Guid estadoCanceladaReservacionId = Guid.NewGuid();

        EstadoReservacion estadoCanceladaReservacion = EstadoReservacion.CrearEstadoReservacionConId(estadoCanceladaReservacionId);

        Reservacion reservacion = new ReservacionTestBuilder()
            .WithId(reservacionId)
            .Build();

        _reservacionRepository.GetReservacionActivaByIdAsync(reservacionId).Returns(reservacion);
        _estadoReservacionRepository.GetEstadoReservacionCanceladaAsync().Returns(estadoCanceladaReservacion);
        _reservacionRepository.CancelarReservacion(reservacion);

        _unitOfWork.SaveAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        Reservacion result = await _service.CancelarReservacionAsync(reservacionId);

        await _reservacionRepository.Received().GetReservacionActivaByIdAsync(Arg.Any<Guid>());
        await _estadoReservacionRepository.Received().GetEstadoReservacionCanceladaAsync();
        _reservacionRepository.Received().CancelarReservacion(Arg.Any<Reservacion>());
        await _unitOfWork.Received().SaveAsync(Arg.Any<CancellationToken>());

        Assert.Equal(estadoCanceladaReservacion, result.EstadoReservacion);
    }
}
