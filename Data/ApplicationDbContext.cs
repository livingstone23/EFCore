using EFCore.Entities.Model;
using EFCore.Entities.Seeding;
using EFCore.Entities.SinLlaves;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Reflection;

namespace EFCore.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {
            
        }



        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveColumnType("date");
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //Pasamos la funcionalidad de la configuracion a un archivo
            //Es mas practico y manejable
            modelBuilder.Entity("GeneroPelicula", b =>
            {
                //b.Property<int>("GenerosIdentificador")
                //    .HasColumnType("int");

                //b.Property<int>("PeliculasId")
                //    .HasColumnType("int");

                //b.HasKey("GenerosIdentificador", "PeliculasId");

                //b.HasIndex("PeliculasId");

                //b.ToTable("GeneroPelicula");
            });

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //Permite centralizar un query
            modelBuilder.Entity<CineSinUbicacion>()
                .HasNoKey().ToSqlQuery("Select Id, Nombre FROM Cines").ToView(null);

            //Llamado a vistas
            modelBuilder.Entity<PeliculaConConteos>().HasNoKey().ToView("PeliculasConConteos");


            //Configuro el modelo, en este caso entidad entidad 
            //Establezco reglas automatica
            //
            foreach (var tipoEntidad in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var propiedad in tipoEntidad.GetProperties())
                {
                    if (propiedad.ClrType == typeof(string) && propiedad.Name.Contains("URL", StringComparison.CurrentCultureIgnoreCase))
                    {
                        propiedad.SetIsUnicode(false);
                        propiedad.SetMaxLength(500);
                    }
                }
            }


            //Agregando llamado al metodo Seed.
            SeedingModuloConsulta.Seed(modelBuilder);


            SeedingPersonaMensaje.Seed(modelBuilder);



            modelBuilder.Entity<Merchandising>().ToTable("Merchandising");
            modelBuilder.Entity<PeliculaAlquilable>().ToTable("PeliculasAlquilables");

            var pelicula1 = new PeliculaAlquilable()
            {
                Id = 1,
                Nombre = "Spider-Man",
                PeliculaId = 1,
                Precio = 5.99m
            };

            var merch1 = new Merchandising()
            {
                Id = 2,
                DisponibleEnInventario = true,
                EsRopa = true,
                Nombre = "T-Shirt One Piece",
                Peso = 1,
                Volumen = 1,
                Precio = 11
            };

            modelBuilder.Entity<Merchandising>().HasData(merch1);
            modelBuilder.Entity<PeliculaAlquilable>().HasData(pelicula1);


        }


        

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cine> Cines { get; set; }
        public DbSet<CineOferta> CinesOfertas { get; set; }
        public DbSet<SalaDeCine> SalasDeCines { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculaActor> PeliculasActores { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<CineSinUbicacion> CinesSinUbicaciones { get; set; }

        public DbSet<Persona> Personas { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }

        public DbSet<CineDetalle> CineDetalle { get; set; }

        public DbSet<Pago> Pagos { get; set; }


        //DBset para tabla por herencia de tipo
        public DbSet<Producto> Productos { get; set; }
    }
}
