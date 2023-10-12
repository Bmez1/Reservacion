using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ReservaHotel.Domain.Entities;
using ReservaHotel.Infrastructure.DataSource;

namespace ReservaHotel.Api.Tests;

internal class ApiApp : WebApplicationFactory<Program>
{
    private const string ESTADORESERVACIONACTIVO = "11111111-1111-1111-1111-111111111111";
    private const string ESTADORESERVACIONCANCELADA = "22222222-2222-2222-2222-222222222222";

    private Guid _clienteId = Guid.NewGuid();
    private Guid _habitacionId = GetHabitacionId();

    public static Guid GetHabitacionId() => Guid.Parse("99999999-9999-9999-9999-999999999999");
    public ApiApp()
    {
    }

    public Guid ClienteId => _clienteId;
    public Guid HabitacionId => _habitacionId;

    public void CrearRegistrosSemillas()
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        Habitacion habitacion = new HabitacionTestBuilder()
            .WithId(_habitacionId)
            .Build();
        Cliente cliente = Cliente.ClienteConId(_clienteId);
        EstadoReservacion estadoReservacionActivo = EstadoReservacion.CrearEstadoReservacionConId(Guid.Parse(ESTADORESERVACIONACTIVO));
        EstadoReservacion estadoReservacionCancelada = EstadoReservacion.CrearEstadoReservacionConId(Guid.Parse(ESTADORESERVACIONCANCELADA));


        dbContext.Set<Habitacion>().Add(habitacion);
        dbContext.Set<Cliente>().Add(cliente);
        dbContext.Set<EstadoReservacion>().AddRange(new EstadoReservacion[] { estadoReservacionActivo, estadoReservacionCancelada });

        dbContext.SaveChanges();
    }

    public void EliminarDB()
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        dbContext.Database.EnsureDeleted();
    }

    // We should use this service collection to access repos and seed data for tests
    public IServiceProvider GetServiceCollection()
    {
        return Services;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(svc =>
        {
            svc.RemoveAll(typeof(DbContextOptions<DataContext>));
            svc.AddDbContext<DataContext>(opt =>
            {
                opt.UseInMemoryDatabase("testdb");
            });
        });
        return base.CreateHost(builder);
    }
}
