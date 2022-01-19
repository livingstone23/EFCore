using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Model
{
    public class Log
    {
        //Para no generar automaticamente el valor.
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public string Mensaje { get; set; }
    }
}
