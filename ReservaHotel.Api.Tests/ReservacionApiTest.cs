using Azure.Core;
using ReservaHotel.Application.Reservaciones;
using ReservaHotel.Domain.Constantes;
using ReservaHotel.Domain.Dto;
using ReservaHotel.Domain.Dtos;
using System.Text.Json;

namespace ReservaHotel.Api.Tests;

public class ReservacionApiTest
{
    private readonly string _baseApiUrlReservacion = "/api/reservacion/";
    private readonly string _baseApiUrlHabitacion = "/api/habitacion/";
    private readonly ApiApp _apiApp;

    public ReservacionApiTest()
    {
        _apiApp = new ApiApp();
    }

    public static IEnumerable<object[]> ConsultaEntidadesFallidasTest => new List<object[]>
        {
            new object[] {"11111111-1111-1111-1111-111111111112", HttpStatusCode.BadRequest},
            new object[] { "11111111-1111-1111-1111-111111111113", HttpStatusCode.BadRequest },
        };

    [Fact]
    public async Task PostReservacionExitosa()
    {
        var reservacionInput = new CreateReservacionCommand()
        {
            ClienteId = _apiApp.ClienteId,
            Comentario = "Reserva exitosa",
            FechaEntrada = DateTime.Now.Date,
            FechaSalida = DateTime.Now.AddDays(5).Date,
            HabitacionId = _apiApp.HabitacionId
        };

        _apiApp.CrearRegistrosSemillas();
        var client = _apiApp.CreateClient();

        var request = await client.PostAsJsonAsync<CreateReservacionCommand>(_baseApiUrlReservacion, reservacionInput);
        request.EnsureSuccessStatusCode();
        var deserializeOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var responseData = JsonSerializer.Deserialize<ReservacionCreadaDto>(await request.Content.ReadAsStringAsync(), deserializeOptions);
        Assert.IsType<ReservacionCreadaDto>(responseData);

        _apiApp.EliminarDB();
    }

    [Fact]
    public async Task CancelarReservacionFallida()
    {
        Guid reservacionId = Guid.NewGuid();
        string mensajeEsperado = $"{reservacionId} no se puede encontrar";

        HttpClient client = _apiApp.CreateClient();

        HttpResponseMessage response = await client.PatchAsync(Path.Combine(_baseApiUrlReservacion, reservacionId.ToString()), null);

        var responseMessage = await response.Content.ReadAsStringAsync();
        Assert.True(response.StatusCode is HttpStatusCode.BadRequest && responseMessage.Contains(mensajeEsperado));
    }

    [Fact]
    public async Task CancelarReservacionExitosa()
    {
        try
        {
            var reservacionInput = new CreateReservacionCommand()
            {
                ClienteId = _apiApp.ClienteId,
                Comentario = "Reserva exitosa",
                FechaEntrada = DateTime.Now.Date,
                FechaSalida = DateTime.Now.AddDays(5).Date,
                HabitacionId = _apiApp.HabitacionId
            };

            _apiApp.CrearRegistrosSemillas();
            var client = _apiApp.CreateClient();

            var request = await client.PostAsJsonAsync<CreateReservacionCommand>(_baseApiUrlReservacion, reservacionInput);
            request.EnsureSuccessStatusCode();

            var deserializeOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var responseData = JsonSerializer.Deserialize<ReservacionCreadaDto>(await request.Content.ReadAsStringAsync(), deserializeOptions);

            var response = await client.PatchAsync(Path.Combine(_baseApiUrlReservacion, responseData!.ReservacionId.ToString()), null);
            response.EnsureSuccessStatusCode();

            var reservacionCancelada = JsonSerializer.Deserialize<ReservacionCanceladaDto>(await response.Content.ReadAsStringAsync(), deserializeOptions);
            Assert.True(reservacionCancelada is not null);
            Assert.IsType<ReservacionCanceladaDto>(reservacionCancelada);

        }
        catch (Exception)
        {
            Assert.True(false, "Ocurrió una excepción");
        }
        finally
        {
            _apiApp.EliminarDB();
        }
    }

    [Fact]
    public async Task PostReservacionFailureFechaPasada()
    {
        string mensajeRespuesta = MensajeValidacionCore.FechaEntradaMenorActual;

        CreateReservacionCommand reservacionInput = new ReservacionCommandTestBuilder()
            .WithFechaEntrada(DateTime.Now.AddDays(-1))
            .WithFechaSalida(DateTime.Now)
            .WithComentario("Reserva fallida")
            .WithClienteId(Guid.Parse("11111111-1111-1111-1111-111111111111"))
            .WithHabitacionId(Guid.Parse("11111111-1111-1111-1111-111111111111"))
            .Build();

        var client = _apiApp.CreateClient();
        HttpResponseMessage request = await client.PostAsJsonAsync<CreateReservacionCommand>(_baseApiUrlReservacion, reservacionInput);

        var responseMessage = await request.Content.ReadAsStringAsync();
        Assert.True(request.StatusCode is HttpStatusCode.UnprocessableEntity && responseMessage.Contains(mensajeRespuesta));

    }

    [Theory]
    [MemberData(nameof(ConsultaEntidadesFallidasTest))]
    public async Task GetHabitacionByIdFallida(string habitacionId, HttpStatusCode httpStatusEsperado)
    {
        string respuestaEsperada = $"{habitacionId} no se puede encontrar";
        var client = _apiApp.CreateClient();

        HttpResponseMessage response = await client.GetAsync(Path.Combine(_baseApiUrlHabitacion, habitacionId));

        var responseMessage = await response.Content.ReadAsStringAsync();

        Assert.True(response.StatusCode == httpStatusEsperado && responseMessage.Contains(respuestaEsperada));
    }

    [Theory]
    [MemberData(nameof(ConsultaEntidadesFallidasTest))]
    public async Task GetReservacionByIdFallida(string reservacionId, HttpStatusCode httpStatusEsperado)
    {
        var client = _apiApp.CreateClient();

        HttpResponseMessage response = await client.GetAsync(Path.Combine(_baseApiUrlReservacion, reservacionId));

        Assert.Equal(httpStatusEsperado, response.StatusCode);
    }

    [Fact]
    public async Task GetHabitacionByIdExitoso()
    {
        _apiApp.CrearRegistrosSemillas();
        try
        {
            HttpResponseMessage response = default!;
            var client = _apiApp.CreateClient();

            response = await client.GetAsync(Path.Combine(_baseApiUrlHabitacion, _apiApp.HabitacionId.ToString()));

            response.EnsureSuccessStatusCode();

            var deserializeOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var responseData = JsonSerializer.Deserialize<HabitacionDto>(await response.Content.ReadAsStringAsync(), deserializeOptions);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(responseData is not null);
            Assert.IsType<HabitacionDto>(responseData);
        }
        catch (HttpRequestException)
        {
            Assert.True(false, "Ocurrió una excepción");
        }
        finally
        {
            _apiApp.EliminarDB();
        }
    }
}
