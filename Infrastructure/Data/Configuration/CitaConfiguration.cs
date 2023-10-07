using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class CitaConfiguration : IEntityTypeConfiguration<Cita>
{
    public void Configure(EntityTypeBuilder<Cita> builder)
    {
        //Here you can configure the properties using the object 'Builder'.
        builder.ToTable("cita");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id);

        builder.Property(c => c.FechaCita).HasColumnType("date");

        builder.Property(c => c.HoraCita).HasColumnType("time");

        builder.HasOne(c => c.Clientes).WithMany(c => c.Citas).HasForeignKey(c => c.IdClienteFk);
        builder.HasOne(c => c.Mascotas).WithMany(c => c.Citas).HasForeignKey(c => c.IdMascotaFk);
        builder.HasOne(c => c.Servicios).WithMany(c => c.Citas).HasForeignKey(c => c.IdServicioFk);
    }
}