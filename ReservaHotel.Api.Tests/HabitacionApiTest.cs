using Newtonsoft.Json;
using ReservaHotel.Application.Habitaciones;
using ReservaHotel.Domain.Constantes;
using ReservaHotel.Domain.Dto;

namespace ReservaHotel.Api.Tests;

public class HabitacionApiTest
{
    private readonly string _baseApiUrlHabitacion = "/api/habitacion/";
    private readonly ApiApp _apiApp;

    public HabitacionApiTest()
    {
        _apiApp = new ApiApp();
    }


    [Fact]
    public async Task ConsultaHabitacionesDisponiblesFallidaFechasPasadas()
    {
        string mensajeRespuesta = MensajeValidacionCore.FechaEntradaMenorActual;

        GetHabitacionesFiltradasQuery habitacionQuery = new GetHabitacionesFiltradasQuery(DateTime.Now.AddDays(-5),
            DateTime.Now.AddDays(-10), Guid.NewGuid());

        string parametrosDeConsulta = $"fechaEntrada={habitacionQuery.fechaEntrada:yyyy-MM-dd}" +
            $"&fechaSalida={habitacionQuery.fechaSalida:yyyy-MM-dd}&tipoHabitacion={habitacionQuery.tipoHabitacion}";

        string urlCompleta = $"{_baseApiUrlHabitacion}filtro?{parametrosDeConsulta}";

        var client = _apiApp.CreateClient();

        HttpResponseMessage request = await client.GetAsync(urlCompleta);

        var responseMessage = await request.Content.ReadAsStringAsync();
        Assert.True(request.StatusCode is HttpStatusCode.BadRequest && responseMessage.Contains(mensajeRespuesta), $"Hora: {DateTime.Now}");

    }

    [Fact]
    public async Task ConsultaHabitacionesDisponiblesExitoso()
    {
        var client = _apiApp.CreateClient();

        HttpResponseMessage request = await client.GetAsync(_baseApiUrlHabitacion);

        request.EnsureSuccessStatusCode();

        var responseMessage = await request.Content.ReadAsStringAsync();
        var habitaciones = JsonConvert.DeserializeObject<List<HabitacionDto>>(responseMessage);

        Assert.Equal(HttpStatusCode.OK, request.StatusCode);
        Assert.IsType<List<HabitacionDto>>(habitaciones);
    }
}
