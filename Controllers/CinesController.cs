using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCore.Data;
using EFCore.Entities.DTOs;
using EFCore.Entities.SinLlaves;
using EFCore.Entities.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace EFCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinesController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CinesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet("SinUbicacion")]
        public async Task<IEnumerable<CineSinUbicacion>> GetCinesSinUbicacion()
        {
            //Permite crear en tiempo real un dbcontext
            //return await context.Set<CineSinUbicacion>().ToListAsync();

            //Llamado atravez de un dataset
            return await context.CinesSinUbicaciones.ToListAsync();

        }


        //Utilizando AutoMapper para mapeo de campos especiales.
        [HttpGet]
        public async Task<IEnumerable<CineDTO>> Get()
        {
            return await context.Cines.ProjectTo<CineDTO>(mapper.ConfigurationProvider).ToListAsync();
        }


        /// <summary>
        /// Ordenando y filtrando campos especiales
        /// Busca a los que estan mas cerca de mi punto a 2 km
        /// </summary>
        /// <param name="latitud"></param>
        /// <param name="longitud"></param>
        /// <returns></returns>
        [HttpGet("cercanos")]
        public async Task<ActionResult> Get(double latitud, double longitud)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            var miUbicacion = geometryFactory.CreatePoint(new Coordinate(longitud, latitud));
            var distanciaMaximaEnMetros = 2000;

            var cines = await context.Cines
                .OrderBy(c => c.Ubicacion.Distance(miUbicacion))
                //Permite establecer un maximo con nuestra distancia.
                .Where(c => c.Ubicacion.IsWithinDistance(miUbicacion, distanciaMaximaEnMetros))
                .Select(c => new
                {
                    Nombre = c.Nombre,
                    Distancia = Math.Round(c.Ubicacion.Distance(miUbicacion))
                }).ToListAsync();

            return Ok(cines);
        }




        /// <summary>
        /// Insertando registros con Data relacionada.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var ubicacionCine = geometryFactory.CreatePoint(new Coordinate(-69.896979, 18.476276));

            var cine = new Cine()
            {
                Nombre = "Mi cine",
                Ubicacion = ubicacionCine,
                CineOferta = new CineOferta()
                {
                    PorcentajeDescuento = 5,
                    FechaInicio = DateTime.Today,
                    FechaFin = DateTime.Today.AddDays(7)
                },
                SalasDeCines = new HashSet<SalaDeCine>()
                {
                    new SalaDeCine()
                    {
                        Precio = 200,
                        TipoSalaDeCine = TipoSalaDeCine.DosDimensiones
                    },
                    new SalaDeCine()
                    {
                        Precio = 350,
                        TipoSalaDeCine = TipoSalaDeCine.TresDimensiones
                    }
                }
            };

            context.Add(cine);
            await context.SaveChangesAsync();
            return Ok();
        }


        /// <summary>
        /// Insertando registro usando DTO.
        /// </summary>
        /// <param name="cineCreacionDTO"></param>
        /// <returns></returns>
        [HttpPost("conDTO")]
        public async Task<ActionResult> Post(CineCreacionDTO cineCreacionDTO)
        {
            var cine = mapper.Map<Cine>(cineCreacionDTO);
            context.Add(cine);
            await context.SaveChangesAsync();
            return Ok();
        }


        //[HttpGet("{id:int}")]
        //public async Task<ActionResult> Get(int id)
        //{
        //    var cineDB = await context.Cines.AsTracking()
        //                   .Include(c => c.SalasDeCine)
        //                   .Include(c => c.CineOferta)
        //                   .FirstOrDefaultAsync(c => c.Id == id);

        //    if (cineDB is null)
        //    {
        //        return NotFound();
        //    }

        //    cineDB.Ubicacion = null;
        //    return Ok(cineDB);
        //}

        //[HttpPut("{id:int}")]
        //public async Task<ActionResult> Put(CineCreacionDTO cineCreacionDTO, int id)
        //{
        //    var cineDB = await context.Cines.AsTracking()
        //                    .Include(c => c.SalasDeCine)
        //                    .Include(c => c.CineOferta)
        //                    .FirstOrDefaultAsync(c => c.Id == id);

        //    if (cineDB is null)
        //    {
        //        return NotFound();
        //    }

        //    cineDB = mapper.Map(cineCreacionDTO, cineDB);
        //    await context.SaveChangesAsync();
        //    return Ok();
        //}

        //[HttpPut("cineOferta")]
        //public async Task<ActionResult> PutCineOferta(CineOferta cineOferta)
        //{
        //    context.Update(cineOferta);
        //    await context.SaveChangesAsync();
        //    return Ok();
        //}


    }
}
