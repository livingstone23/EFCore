using EFCore.Entities.Model;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore.Entities.Conversions
{
    /// <summary>
    /// Clase que permite mapear de simbolo a enum
    /// </summary>
    public class MonedaASimboloConverter : ValueConverter<Moneda, string>
    {

        public MonedaASimboloConverter() : base(valor => MapeoMonedaString(valor), valor => MapeoStringMoneda(valor))
        {

        }

        /// <summary>
        /// Mapeo de Moneda --> String
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        private static string MapeoMonedaString(Moneda valor)
        {
            return valor switch
            {
                Moneda.PesoDominicano => "RD$",
                Moneda.DolarEstadounidense => "$",
                Moneda.Euro => "€",  
                _ => ""
            };
        }

        /// <summary>
        /// Mapeo de String --> Moneda
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        private static Moneda MapeoStringMoneda(string valor)
        {
            return valor switch
            {
                "RD$" => Moneda.PesoDominicano,
                "$" => Moneda.DolarEstadounidense,
                "€" => Moneda.Euro,
                _ => Moneda.Desconocida
            };
        }

    }
}
