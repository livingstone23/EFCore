using Microsoft.EntityFrameworkCore;

namespace EFCore.Model
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public bool EnCartelera { get; set; }
        public DateTime FechaEstreno { get; set; }
        
        //las url no utilizan caracteres especiales por ende el uso
        //de unicode seria falso.
        [Unicode(false)]
        public string PosterURL { get; set; }
        public HashSet<Genero> Generos { get; set; }
        public  HashSet<SalaDeCine> SalasDeCine { get; set; }
        public  HashSet<PeliculaActor> PeliculasActores { get; set; }
    }
}
