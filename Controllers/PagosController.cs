
using EFCore.Data;
using EFCore.Entities.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Controllers
{
    [ApiController]
    [Route("api/pagos")]
    public class PagosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public PagosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pago>>> Get()
        {
            return await context.Pagos.ToListAsync();
        }

        /// <summary>
        /// con esta funcion solo trae los pagos que tienen vinculacion con tarjeta
        /// por la estructura de herencia
        /// </summary>
        /// <returns></returns>
        [HttpGet("tarjetas")]
        public async Task<ActionResult<IEnumerable<PagoTarjeta>>> GetTarjetas()
        {
            return await context.Pagos.OfType<PagoTarjeta>().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("paypal")]
        public async Task<ActionResult<IEnumerable<PagoPaypal>>> GetPaypal()
        {
            return await context.Pagos.OfType<PagoPaypal>().ToListAsync();
        }
    }

}
