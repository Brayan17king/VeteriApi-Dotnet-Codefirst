using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            // Aquí puedes configurar las propiedades de la entidad Marca
            // utilizando el objeto 'builder'.
            builder.ToTable("cliente");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);

            builder.Property(p => p.NombreCliente)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(p => p.ApellidoCliente)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(p => p.EmailCliente)
            .IsRequired()
            .HasMaxLength(80);
        }
    }
}