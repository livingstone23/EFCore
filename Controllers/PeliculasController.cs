using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCore.Data;
using EFCore.Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;


        public PeliculasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }



        /// <summary>
        /// Traer data relacionada utilizando Eager Loading
        /// Sin utilizar la funcion de projectTo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas
                .Include(p => p.Generos.OrderByDescending(g => g.Nombre))
                .Include(p => p.SalasDeCine)
                    .ThenInclude(s => s.Cine)
                .Include(p => p.PeliculasActores.Where(pa => pa.Actor.FechaNacimiento.Value.Year >= 1980))
                    .ThenInclude(pa => pa.Actor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);

            peliculaDTO.Cines = peliculaDTO.Cines.DistinctBy(x => x.Id).ToList();

            return peliculaDTO;
        }



        /// <summary>
        /// Con Projecto y Eager Loading
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("conprojectto/{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> GetProjectTo(int id)
        {
            var pelicula = await context.Peliculas
                .ProjectTo<PeliculaDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            pelicula.Cines = pelicula.Cines.DistinctBy(x => x.Id).ToList();

            return pelicula;
        }


        /// <summary>
        /// Otra Manera de cargar valores de manera selectiva.
        /// Utilizando la funcion select.
        /// Es la utilizacion del Select de manera precisa, mas eficiente que el cargador selectivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("cargadoselectivo/{id:int}")]
        public async Task<ActionResult> GetSelectivo(int id)
        {
            var pelicula = await context.Peliculas.Select(p =>
            new
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Generos = p.Generos.OrderByDescending(g => g.Nombre).Select(g => g.Nombre).ToList(),
                CantidadActores = p.PeliculasActores.Count(),
                CantidadCines = p.SalasDeCine.Select(s => s.CineId).Distinct().Count()
            }).FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            return Ok(pelicula);
        }



        /// <summary>
        /// Carga explicito
        /// Utilizando el AsTracking
        /// Son pocos eficientes, por realizar diferentes consultas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("cargadoexplicito/{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> GetExplicito(int id)
        {
            var pelicula = await context.Peliculas.AsTracking().FirstOrDefaultAsync(p => p.Id == id);
            //...

            //Esta seccion realiza la carga de la coleccion, esto permite realizar cargas separadas
            //await context.Entry(pelicula).Collection(p => p.Generos).LoadAsync();

            var cantidadGeneros = await context.Entry(pelicula).Collection(p => p.Generos).Query().CountAsync();

            if (pelicula is null)
            {
                return NotFound();
            }

            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);

            return peliculaDTO;
        }


        /// <summary>
        /// Metodo Lazyloading
        /// Si la data no ha sido cargada, busca esta en la base de datos, si esta ya se encuentra en memoria solo la sustrae.
        /// mas ineficiente que el cargador selectivo.
        /// </summary>
        /// <returns></returns>
        [HttpGet("lazyloading/{id:int}")]
        public async Task<ActionResult<List<PeliculaDTO>>> GetLazyLoading()
        {
            var peliculas = await context.Peliculas.AsTracking().ToListAsync();

            foreach (var pelicula in peliculas)
            {
                // cargar los generos de la pelicula

                // Problema n + 1
                pelicula.Generos.ToList();
            }

            var peliculasDTOs = mapper.Map<List<PeliculaDTO>>(peliculas);
            return peliculasDTOs;
        }

        //[HttpGet("agrupadasPorEstreno")]
        //public async Task<ActionResult> GetAgrupadasPorCartelera()
        //{
        //    var peliculasAgrupadas = await context.Peliculas.GroupBy(p => p.EnCartelera)
        //                                .Select(g => new
        //                                {
        //                                    EnCartelera = g.Key,
        //                                    Conteo = g.Count(),
        //                                    Peliculas = g.ToList()
        //                                }).ToListAsync();

        //    return Ok(peliculasAgrupadas);
        //}

        //[HttpGet("agrupadasPorCantidadDeGeneros")]
        //public async Task<ActionResult> GetAgrupadasPorCantidadDeGeneros()
        //{
        //    var peliculasAgrupadas = await context.Peliculas.GroupBy(p => p.Generos.Count())
        //                            .Select(g => new
        //                            {
        //                                Conteo = g.Key,
        //                                Titulos = g.Select(x => x.Titulo),
        //                                Generos = g.Select(p => p.Generos)
        //                                        .SelectMany(gen => gen).Select(gen => gen.Nombre).Distinct()
        //                            }).ToListAsync();

        //    return Ok(peliculasAgrupadas);
        //}

        //[HttpGet("filtrar")]
        //public async Task<ActionResult<List<PeliculaDTO>>> Filtrar(
        //        [FromQuery] PeliculasFiltroDTO peliculasFiltroDTO)
        //{
        //    var peliculasQueryable = context.Peliculas.AsQueryable();

        //    if (!string.IsNullOrEmpty(peliculasFiltroDTO.Titulo))
        //    {
        //        peliculasQueryable = peliculasQueryable.Where(p => p.Titulo.Contains(peliculasFiltroDTO.Titulo));
        //    }

        //    if (peliculasFiltroDTO.EnCartelera)
        //    {
        //        peliculasQueryable = peliculasQueryable.Where(p => p.EnCartelera);
        //    }

        //    if (peliculasFiltroDTO.ProximosEstrenos)
        //    {
        //        var hoy = DateTime.Today;
        //        peliculasQueryable = peliculasQueryable.Where(p => p.FechaEstreno > hoy);
        //    }

        //    if (peliculasFiltroDTO.GeneroId != 0)
        //    {
        //        peliculasQueryable = peliculasQueryable.Where(p =>
        //            p.Generos.Select(g => g.Identificador)
        //                    .Contains(peliculasFiltroDTO.GeneroId));
        //    }

        //    var peliculas = await peliculasQueryable.Include(p => p.Generos).ToListAsync();

        //    return mapper.Map<List<PeliculaDTO>>(peliculas);
        //}

    }
}
