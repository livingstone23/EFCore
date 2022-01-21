using Microsoft.EntityFrameworkCore;

namespace EFCore.Entities.Model
{
    public abstract class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        [Precision(18, 2)]
        public decimal Precio { get; set; }
    }
}
