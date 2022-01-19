using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EFCore.Entities.Model
{
    //Debe marcarse para volverla una entidad de propiedad.
    //Al menos una propiedad debe ser obligatoria.
    [Owned]
    public class Direccion
    {
        public string Calle { get; set; }
        public string Provincia { get; set; }
        [Required]
        public string Pais { get; set; }
    }
}
