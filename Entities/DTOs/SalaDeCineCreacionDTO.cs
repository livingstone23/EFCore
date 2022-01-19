using EFCore.Entities.DTOs;
using EFCore.Entities.Model;

namespace EFCorePeliculas.DTOs
{
    public class SalaDeCineCreacionDTO
    {
        public int Id { get; set; }
        public decimal Precio { get; set; }
        public TipoSalaDeCine TipoSalaDeCine { get; set; }
    }
}
