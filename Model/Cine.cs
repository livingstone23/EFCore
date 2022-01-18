using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Model
{
    public class Cine
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        
        public Point Ubicacion { get; set; }
        public CineOferta CineOferta { get; set; }

        //Indicamos que son varias ofertas
        //Mas rapido que ICollection pero desventaja es que no ordena
        //[ForeignKey("CineId")]
        public HashSet<SalaDeCine> SalasDeCines { get; set; }
    }
}
