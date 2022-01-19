using EFCore.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Entities.Configurations
{
    public class CineConfig : IEntityTypeConfiguration<Cine>
    {
        public void Configure(EntityTypeBuilder<Cine> builder)
        {
            builder.Property(prop => prop.Nombre)
              .HasMaxLength(150)
              .IsRequired();


            //Defino que existe una unica relacion
            builder.HasOne(c => c.CineOferta)
                .WithOne()
                .HasForeignKey<CineOferta>(co => co.CineId);

            //Una relacion de 1 a muchos
            //Se haplica el tipo de borrado Restrict, tambien
            //se pudo haber utilizado NoAction.
            //Con restrict obliga a que se borre primera la sala de cine.
            builder.HasMany(c => c.SalasDeCines)
                .WithOne(s => s.Cine)
                .HasForeignKey(s => s.CineId)
                .OnDelete(DeleteBehavior.Restrict);

            //Configuro la relacion 1 a -> a para aplicar 
            //division de tabla.
            builder.HasOne(c => c.CineDetalle).WithOne(cd => cd.Cine)
                .HasForeignKey<CineDetalle>(cd => cd.Id);

            //Indico con la propiedad que direcion es parte de la propiedad
            //de actor y le controlo como nombrar las propiedades.
            builder.OwnsOne(c => c.Direccion, dir =>
            {
                dir.Property(d => d.Calle).HasColumnName("Calle");
                dir.Property(d => d.Provincia).HasColumnName("Provincia");
                dir.Property(d => d.Pais).HasColumnName("Pais");
            });
        }
    }
}
