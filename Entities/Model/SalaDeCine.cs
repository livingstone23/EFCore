using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Entities.Model
{
    public class SalaDeCine
    {
        public int Id { get; set; }

        //ESTABLECE VALORES EN BASE A ENUMERACION, VER CONFIGURACION SalaDeCineConfig
        public TipoSalaDeCine TipoSalaDeCine { get; set; }
        public decimal Precio { get; set; }

        
        public int CineId { get; set; }

        [ForeignKey(nameof(CineId))]
        public Cine Cine { get; set; }

        public HashSet<Pelicula> Peliculas { get; set; }

        public Moneda Moneda { get; set; }

    }
}

