using EFCore.Entities.Seeding;
using EFCore.Entities.SinLlaves;
using EFCore.Model;
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

            //Configurando con el Apifluenti
            modelBuilder.Entity("EFCore.Model.Actor", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                b.Property<string>("Biografia")
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime?>("FechaNacimiento")
                    .HasColumnType("date");

                b.Property<string>("Nombre")
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnType("nvarchar(150)");

                b.HasKey("Id");

                b.ToTable("Actores");
            });

            modelBuilder.Entity("EFCore.Model.Cine", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                b.Property<string>("Nombre")
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnType("nvarchar(150)");

                b.Property<Point>("Ubicacion")
                    .HasColumnType("geography");

                b.HasKey("Id");

                b.ToTable("Cines");
            });

            modelBuilder.Entity("EFCore.Model.CineOferta", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                b.Property<int>("CineId")
                    .HasColumnType("int");

                b.Property<DateTime>("FechaFin")
                    .HasColumnType("date");

                b.Property<DateTime>("FechaInicio")
                    .HasColumnType("date");

                b.Property<decimal>("PorcentajeDescuento")
                    .HasPrecision(5, 2)
                    .HasColumnType("decimal(5,2)");

                b.HasKey("Id");

                b.HasIndex("CineId")
                    .IsUnique();

                b.ToTable("CinesOfertas");
            });

            modelBuilder.Entity("EFCore.Model.Genero", b =>
            {
                b.Property<int>("Identificador")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Identificador"), 1L, 1);

                b.Property<string>("Nombre")
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnType("nvarchar(150)");

                b.HasKey("Identificador");

                b.ToTable("Generos");
            });

            modelBuilder.Entity("EFCore.Model.Pelicula", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                b.Property<bool>("EnCartelera")
                    .HasColumnType("bit");

                b.Property<DateTime>("FechaEstreno")
                    .HasColumnType("date");

                b.Property<string>("PosterURL")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnType("varchar(500)");

                b.Property<string>("Titulo")
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnType("nvarchar(250)");

                b.HasKey("Id");

                b.ToTable("Peliculas");
            });

            modelBuilder.Entity("EFCore.Model.PeliculaActor", b =>
            {
                b.Property<int>("PeliculaId")
                    .HasColumnType("int");

                b.Property<int>("ActorId")
                    .HasColumnType("int");

                b.Property<int>("Orden")
                    .HasColumnType("int");

                b.Property<string>("Personaje")
                    .HasMaxLength(150)
                    .HasColumnType("nvarchar(150)");

                b.HasKey("PeliculaId", "ActorId");

                b.HasIndex("ActorId");

                b.ToTable("PeliculasActores");
            });

            modelBuilder.Entity("EFCore.Model.SalaDeCine", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                //b.Property<decimal>("Precio")
                //    .HasPrecision(9, 2)
                //    .HasColumnType("decimal(9,2)");

                //b.Property<int>("TipoSalaDeCine")
                //    .ValueGeneratedOnAdd()
                //    .HasColumnType("int")
                //    .HasDefaultValue(1);

                //b.HasKey("Id");

                //b.ToTable("SalasDeCine");
            });

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


            //Agregando llamado al metodo Seed.
            SeedingModuloConsulta.Seed(modelBuilder);
           
        }


        public DbSet<Persona> Personas { get; set; }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cine> Cines { get; set; }
        public DbSet<CineOferta> CinesOfertas { get; set; }
        public DbSet<SalaDeCine> SalasDeCines { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculaActor> PeliculasActores { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<CineSinUbicacion> CinesSinUbicaciones { get; set; }
    }
}
