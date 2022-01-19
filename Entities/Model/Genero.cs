using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Entities.Model
{
    //[Table("Generos", Schema="Peliculas")]
    [Index(nameof(Nombre), IsUnique = true)]
    public class Genero
    {
        //[Key]
        public int Identificador { get; set; }
        //[StringLength(150)]
        //[MaxLength(150)]
        //[Required]
        //[Column("NombreGenero")]
        public string Nombre { get; set; }

        public bool EstaBorrado { get; set; }
        public HashSet<Pelicula> Peliculas { get; set; }
    }
}
