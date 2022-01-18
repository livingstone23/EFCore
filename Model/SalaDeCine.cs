using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Model
{
    public class SalaDeCine
    {
        public int Id { get; set; }
        public int TipoSalaDeCine { get; set; }
        public decimal Precio { get; set; }

        
        public int CineId { get; set; }
        public Cine Cine { get; set; }

        public HashSet<Pelicula> Peliculas { get; set; }
    }
}
