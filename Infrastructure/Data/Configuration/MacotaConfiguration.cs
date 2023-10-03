using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class MascotaConfiguration : IEntityTypeConfiguration<Mascota>
    {
        public void Configure(EntityTypeBuilder<Mascota> builder)
        {
            // Aquí puedes configurar las propiedades de la entidad Marca
            // utilizando el objeto 'builder'.
            builder.ToTable("mascota");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);

            builder.Property(p => p.NombreMascota)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(p => p.FechaNacimientoMascota)
            .HasColumnType("datetime");

            builder.HasOne(p => p.Razas)
            .WithMany(p => p.Mascotas)
            .HasForeignKey(p => p.IdRazaFk);

            builder.HasOne(p => p.Clientes)
            .WithMany(p => p.Mascotas)
            .HasForeignKey(p => p.IdClienteFk);
        }
    }
}