using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;
using ReservaHotel.Api.ApiHandlers.EndPoints;
using ReservaHotel.Api.Filters;
using ReservaHotel.Api.Middleware;
using ReservaHotel.Domain.Configuration;
using ReservaHotel.Infrastructure.DataSource;
using ReservaHotel.Infrastructure.Extensions;
using Serilog;
using Serilog.Debugging;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(config.GetConnectionString("db"));
});

builder.Services.Configure<InformacionEstadoEntidad>(config.GetSection("InformacionEstadoEntidad"));

builder.Services.AddHealthChecks()
    .AddDbContextCheck<DataContext>()
    .ForwardToPrometheus();

builder.Services.AddDomainServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(Assembly.Load("ReservaHotel.Application"), typeof(Program).Assembly);

builder.Host.UseSerilog((_, loggerConfiguration) =>
    loggerConfiguration
        .WriteTo.Console());

SelfLog.Enable(Console.Error);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpMetrics();

app.UseMiddleware<AppExceptionHandlerMiddleware>();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.UseRouting().UseEndpoints(endpoint =>
{
    endpoint.MapMetrics();
});

app.MapGroup("/api/reservacion")
    .WithTags("Reservaciones")
    .MapReservacion().AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);

app.MapGroup("/api/habitacion")
    .WithTags("Habitaciones")
    .MapHabitacion().AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);

app.Run();
