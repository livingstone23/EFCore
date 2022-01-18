using AutoMapper;
using EFCore.Entities.DTOs;
using EFCore.Model;

namespace EFCore.Service
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Actor, ActorDTO>();


            //Mapeo de campos especiales.
            CreateMap<Cine, CineDTO>()
                .ForMember(dto => dto.Latitud, ent => ent.MapFrom(prop => prop.Ubicacion.Y))
                .ForMember(dto => dto.Longitud, ent => ent.MapFrom(prop => prop.Ubicacion.X));

            CreateMap<Genero, GeneroDTO>();

            // Sin projectTo
            //CreateMap<Pelicula, PeliculaDTO>()
            //    .ForMember(dto => dto.Cines, ent => ent.MapFrom(prop => prop.SalasDeCine.Select(s => s.Cine)))
            //    .ForMember(dto => dto.Actores, ent =>
            //        ent.MapFrom(prop => prop.PeliculasActores.Select(pa => pa.Actor)));

            // Con ProjectTo
            CreateMap<Pelicula, PeliculaDTO>()
                .ForMember(dto => dto.Generos, ent => ent.MapFrom(prop =>
                    prop.Generos.OrderByDescending(g => g.Nombre)))
                .ForMember(dto => dto.Cines, ent => ent.MapFrom(prop => prop.SalasDeCine.Select(s => s.Cine)))
                .ForMember(dto => dto.Actores, ent =>
                    ent.MapFrom(prop => prop.PeliculasActores.Select(pa => pa.Actor)));


        }
    }
}
