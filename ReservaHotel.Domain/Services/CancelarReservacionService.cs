using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Ports;

namespace ReservaHotel.Domain.Services;

[DomainService]
public class CancelarReservacionService
{
    private readonly IReservacionRepository _reservacionRepository;
    private readonly IEstadoReservacionRepository _estadoReservacionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelarReservacionService(IReservacionRepository reservacionRepository, 
        IEstadoReservacionRepository estadoReservacionRepository, IUnitOfWork unitOfWork)
    {
        (_reservacionRepository, _unitOfWork, _estadoReservacionRepository) =
            (reservacionRepository, unitOfWork, estadoReservacionRepository);
    }

    public async Task<Reservacion> CancelarReservacionAsync(Guid reservacionId, CancellationToken? cancellationToken = null)
    {
        var token = cancellationToken ?? new CancellationTokenSource().Token;

        Reservacion reservacion = await ConsultarReservacionAsync(reservacionId);
        EstadoReservacion estadoReservacionCancelada = await _estadoReservacionRepository.GetEstadoReservacionCanceladaAsync();
        DateTime fechaCancelacion = DateTime.Now;
        reservacion.CalcularPrecioReservacion(fechaCancelacion);

        reservacion.EstadoReservacion = estadoReservacionCancelada;

        _reservacionRepository.CancelarReservacion(reservacion);

        await _unitOfWork.SaveAsync(token);
        return reservacion;
    }

    private async Task<Reservacion> ConsultarReservacionAsync(Guid reservacionId)
    {
        return await _reservacionRepository.GetReservacionActivaByIdAsync(reservacionId);
    }
}
