using EFCore.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Entities.Configurations
{

    //Entidad intermedia
    public class PeliculaActorConfig : IEntityTypeConfiguration<PeliculaActor>
    {
        public void Configure(EntityTypeBuilder<PeliculaActor> builder)
        {
            builder.HasKey(prop =>
             new { prop.PeliculaId, prop.ActorId });

            //Relacion 1 a Muchos
            builder.HasOne(pa => pa.Actor)
                    .WithMany(a => a.PeliculasActores)
                    .HasForeignKey(pa => pa.ActorId);

            //Relacion 1 a Muchos
            builder.HasOne(pa => pa.Pelicula)
                    .WithMany(p => p.PeliculasActores)
                    .HasForeignKey(pa => pa.PeliculaId);


            builder.Property(prop => prop.Personaje)
                .HasMaxLength(150);
        }
    }
}
