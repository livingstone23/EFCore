using EFCore.Data;
using EFCore.Entities.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {

        private readonly ApplicationDbContext context;

        public PersonasController(ApplicationDbContext context)
        {
            this.context = context;
        }


        /// <summary>
        /// Permite un llamado a una clase con dos llaves foraneas relacionadas.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Persona>> Get(int id)
        {
            return await context.Personas
                        .Include(p => p.MensajesEnviados)
                        .Include(p => p.MensajesRecibidos)
                        .FirstOrDefaultAsync(p => p.Id == id);
        }

    }
}
