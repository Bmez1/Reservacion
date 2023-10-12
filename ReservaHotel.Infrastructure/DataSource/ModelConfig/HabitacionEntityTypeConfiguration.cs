using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservaHotel.Domain.Entities;

namespace ReservaHotel.Infrastructure.DataSource.ModelConfig;

public class HabitacionEntityTypeConfiguration : IEntityTypeConfiguration<Habitacion>
{
    public void Configure(EntityTypeBuilder<Habitacion> builder)
    {
        builder
            .HasOne(r => r.EstadoHabitacion)
            .WithMany(c => c.Habitaciones)
            .HasForeignKey("EstadoHabitacionId")
            .IsRequired();

        builder
            .HasOne(r => r.TipoHabitacion)
            .WithMany(c => c.Habitaciones)
            .HasForeignKey("TipoHabitacionId")
            .IsRequired();
    }
}
