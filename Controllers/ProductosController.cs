using EFCore.Data;
using EFCore.Entities.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {

        private readonly ApplicationDbContext context;

        public ProductosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> Get()
        {
            return await context.Productos.ToListAsync();
        }


        /// <summary>
        /// Metodo que llama tabla de tipo herencia, utiliza el set
        /// </summary>
        /// <returns></returns>
        [HttpGet("Merchs")]
        public async Task<ActionResult<IEnumerable<Merchandising>>> GetMerchs()
        {
            return await context.Set<Merchandising>().ToListAsync();
        }



        /// <summary>
        /// Metodo que llama tabla de tipo herencia, utiliza el set
        /// </summary>
        /// <returns></returns>
        [HttpGet("Alquileres")]
        public async Task<ActionResult<IEnumerable<PeliculaAlquilable>>> GetAlquileres()
        {
            return await context.Set<PeliculaAlquilable>().ToListAsync();
        }

    }
}
