using ReservaHotel.Domain.Constantes;
using ReservaHotel.Domain.Exceptions;
using ReservaHotel.Domain.QueryFiltros;

namespace ReservaHotel.Domain.Tests;

public class HabitacionTests
{
    public static IEnumerable<object[]> ReservacionesTest => new List<object[]>
        {
            new object[]
            {
                new HabitacionesFiltradasQueryTestBuilder()
                    .WithFechaEntrada(DateTime.Now.AddDays(-10)),
                MensajeValidacionCore.FechaEntradaMenorActual
            },
            new object[]
            {
                new HabitacionesFiltradasQueryTestBuilder()
                    .WithFechaEntrada(DateTime.Now.Date)
                    .WithFechaSalida (DateTime.Now.Date),
                MensajeValidacionCore.FechaSalidaMenorActual
            },
            new object[]
            {
                new HabitacionesFiltradasQueryTestBuilder()
                .WithFechaEntrada(DateTime.Now.AddDays(-1)),
                MensajeValidacionCore.FechaEntradaMenorActual
            },
            new object[]
            {
                new HabitacionesFiltradasQueryTestBuilder()
                .WithFechaEntrada(DateTime.Now.AddDays(5))
                .WithFechaSalida(DateTime.Now.AddDays(4)),
                MensajeValidacionCore.FechaSalidaMenorEntrada
            },
        };


    [Theory]
    [MemberData(nameof(ReservacionesTest))]
    public void CrearFiltroHabitacionTestFallido(HabitacionesFiltradasQueryTestBuilder filtroHabitacion, string response)
    {
        var error = Record.Exception(() => filtroHabitacion.Build());

        Assert.IsType<CoreBusinessException>(error);
        Assert.Equal(response, error.Message);
    }

    [Fact]
    public void CrearFiltroConTipoHabitacion()
    {
        DateTime fechaEntrada = DateTime.Now.Date.AddDays(1);
        DateTime fechaSalida = DateTime.Now.Date.AddDays(3);
        Guid tipoHabitacion = Guid.NewGuid();

        var filtro = new FiltroBusquedaHabitacion(fechaEntrada, fechaSalida, tipoHabitacion);

        Assert.Equal(tipoHabitacion, filtro.TipoHabitacion);
        Assert.Equal(fechaEntrada, filtro.FechaEntrada);
        Assert.Equal(fechaSalida, filtro.FechaSalida);
    }
}
