using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservaHotel.Domain.Entities;

namespace ReservaHotel.Infrastructure.DataSource.ModelConfig;

public class ReservacionEntityTypeConfiguration : IEntityTypeConfiguration<Reservacion>
{
    public void Configure(EntityTypeBuilder<Reservacion> builder)
    {
        builder
            .HasOne(r => r.Cliente)
            .WithMany(c => c.Reservaciones)
            .HasForeignKey("ClienteId")
            .IsRequired();

        builder
            .HasOne(r => r.EstadoReservacion)
            .WithMany(c => c.Reservaciones)
            .HasForeignKey("EstadoReservacionId")
            .IsRequired();

        builder
            .HasOne(r => r.Habitacion)
            .WithMany(c => c.Reservaciones)
            .HasForeignKey("HabitacionId")
            .IsRequired();
    }
}
