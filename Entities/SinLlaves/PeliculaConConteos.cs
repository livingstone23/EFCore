﻿namespace EFCore.Entities.SinLlaves
{
    //Clase para mepear una vista
    public class PeliculaConConteos
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int CantidadGeneros { get; set; }
        public int CantidadCines { get; set; }
        public int CantidadActores { get; set; }

    }
}
