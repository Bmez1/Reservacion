using Microsoft.Extensions.Options;
using ReservaHotel.Domain.Configuration;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Domain.Exceptions;
using ReservaHotel.Domain.Ports;
using ReservaHotel.Infrastructure.Ports;

namespace ReservaHotel.Infrastructure.Adapters
{
    [Repository]
    public class EstadoReservacionRepository : IEstadoReservacionRepository
    {
        private readonly BaseRepositoryGeneric<EstadoReservacion> _dataSource;
        private readonly InformacionEstadoEntidad _informacionEstadoEntidad;

        public EstadoReservacionRepository(BaseRepositoryGeneric<EstadoReservacion> dataSource, IOptions<InformacionEstadoEntidad> confi)
        {
            _dataSource = dataSource;
            _informacionEstadoEntidad = confi?.Value
                ?? throw new ArgumentNullException(nameof(confi));
        }

        public Task<EstadoReservacion> GetEstadoReservacionActivoAsync()
        {
            return _dataSource.GetOneAsync(_informacionEstadoEntidad.EstadoActivoReservacionId);
        }

        public Task<EstadoReservacion> GetEstadoReservacionCanceladaAsync()
        {
            return _dataSource.GetOneAsync(_informacionEstadoEntidad.EstadoCanceladoReservacionId);
        }
    }
}
