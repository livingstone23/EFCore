using EFCore.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Entities.Configurations
{
    //Clase Padre para aplicar Herencia.
    public class PagoConfig : IEntityTypeConfiguration<Pago>
    {
        public void Configure(EntityTypeBuilder<Pago> builder)
        {
            //HasDiscriminator --Permita determinar a que tabla hijo va a utilizar
            builder.HasDiscriminator(p => p.TipoPago)
                .HasValue<PagoPaypal>(TipoPago.Paypal)
                .HasValue<PagoTarjeta>(TipoPago.Tarjeta);

            //Defino la precision del monto ha pagar.
            builder.Property(p => p.Monto).HasPrecision(18, 2);
        }
    }
}
