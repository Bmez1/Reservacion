using ReservaHotel.Domain.Constantes;
using ReservaHotel.Domain.Dtos.Inputs;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Exceptions;
using ReservaHotel.Domain.Ports;

namespace ReservaHotel.Domain.Services;

[DomainService]
public class CreateReservacionService
{
    private const int CANTIDADMAXIMARESERVACION = 2;
    private readonly IReservacionRepository _reservacionRepository;
    private readonly IHabitacionRepository _habitacionRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IEstadoReservacionRepository _estadoReservacionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReservacionService(IReservacionRepository reservacionRepository,
        IHabitacionRepository habitacionRepository, IClienteRepository clienteRepository,
        IEstadoReservacionRepository estadoReservacionRepository, IUnitOfWork unitOfWork)
    {
        (_reservacionRepository, _unitOfWork, _habitacionRepository, _clienteRepository, _estadoReservacionRepository) =
            (reservacionRepository, unitOfWork, habitacionRepository, clienteRepository, estadoReservacionRepository);
    }

    public async Task<Reservacion> RegistrarReservacionAsync(CreateReservacionDto reservacionInput, CancellationToken? cancellationToken = null)
    {
        var token = cancellationToken ?? new CancellationTokenSource().Token;
        await VerificarCantidadReservasActivasAsync(reservacionInput!.clienteId);

        Habitacion habitacion = await _habitacionRepository.GetHabitacionByIdAsync(reservacionInput.habitacionId);
        Cliente cliente = await _clienteRepository.GetClienteByIdAsync(reservacionInput.clienteId);
        EstadoReservacion estadoActivoReservacion = await _estadoReservacionRepository.GetEstadoReservacionActivoAsync();

        var reservacion = new Reservacion(reservacionInput.fechaEntrada, reservacionInput.fechaSalida,
            cliente, habitacion, estadoActivoReservacion);

        reservacion.Comentario = reservacionInput.comentario;

        var reservacionCreada = await _reservacionRepository.CrearReservacionAsync(reservacion);
        await _unitOfWork.SaveAsync(token);
        return reservacionCreada;
    }

    private async Task VerificarCantidadReservasActivasAsync(Guid clienteId)
    {
        int cantidadReservasACtivas = await _reservacionRepository.GetCantidadReservasActivasByPersonaAsync(clienteId);
        if (cantidadReservasACtivas > CANTIDADMAXIMARESERVACION)
        {
            throw new CoreBusinessException(MensajeValidacionCore.ClienteConMasDe2Reservaciones);
        }
    }
}
