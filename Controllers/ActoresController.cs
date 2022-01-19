using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCore.Data;
using EFCore.Entities.DTOs;
using EFCore.Model;
using EFCorePeliculas.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActoresController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ActoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //Utilizando autoMapper
        [HttpGet]
        public async Task<IEnumerable<ActorDTO>> Get()
        {
            return await context.Actores.ProjectTo<ActorDTO>(mapper.ConfigurationProvider).ToListAsync();
        }


        //Sin utilizar AutoMapper
        //[HttpGet]
        //public async Task<IEnumerable<ActorDTO>> Get()
        //{
        //    return await context.Actores.Select(a => new ActorDTO { Id = a.Id, Nombre = a.Nombre }).ToListAsync();
        //}



        /// <summary>
        /// Mapeo flexible.
        /// </summary>
        /// <param name="actorCreacionDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(ActorCreacionDTO actorCreacionDTO)
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);
            context.Add(actor);
            await context.SaveChangesAsync();
            return Ok();
        }



        /// <summary>
        /// Actualizando registro con el modelo conectado.
        /// Es un metodo mas eficiente.
        /// </summary>
        /// <param name="actorCreacionDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(ActorCreacionDTO actorCreacionDTO, int id)
        {
            var actorDB = await context.Actores.AsTracking().FirstOrDefaultAsync(a => a.Id == id);

            if (actorDB is null)
            {
                return NotFound();
            }

            //Con la linea se mapea de un objeto DTO --> DB
            actorDB = mapper.Map(actorCreacionDTO, actorDB);
            await context.SaveChangesAsync();
            return Ok();
        }



        /// <summary>
        /// Actualizando registro con el modelo desconectado.
        /// Actualiza todo incluyendo lo que no sufrio cambio.
        /// es mas eficiente el modelo conectado.
        /// </summary>
        /// <param name="actorCreacionDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("desconectado/{id:int}")]
        public async Task<ActionResult> PutDesconectado(ActorCreacionDTO actorCreacionDTO, int id)
        {
            var existeActor = await context.Actores.AnyAsync(a => a.Id == id);

            if (!existeActor)
            {
                return NotFound();
            }

            var actor = mapper.Map<Actor>(actorCreacionDTO);
            actor.Id = id;

            context.Update(actor);
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
