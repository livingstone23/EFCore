﻿using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Model
{
    [NotMapped]
    public class Direccion
    {
        public string Calle { get; set; }
        public string Provincia { get; set; }
        public string Pais { get; set; }
    }
}
