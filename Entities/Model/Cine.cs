using NetTopologySuite.Geometries;

namespace EFCore.Entities.Model
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

        //Propiedad de navegacion utilizada 
        //para la division de tabla.
        public CineDetalle CineDetalle { get; set; }
        public Direccion Direccion { get; set; }
    }
}
