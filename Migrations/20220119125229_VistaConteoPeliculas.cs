using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.Migrations
{
    public partial class VistaConteoPeliculas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Moneda",
                table: "SalasDeCines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");


            migrationBuilder.Sql(@"
                CREATE VIEW [dbo].[PeliculasConConteos]
                AS
                Select Id, Titulo,
                (Select count(*)
                from GeneroPelicula
                WHERE PeliculasId = Peliculas.Id) as CantidadGeneros,
                (Select count(distinct CineId)
                FROM PeliculaSalaDeCine
                INNER JOIN SalasDeCines
                ON SalasDeCines.Id = PeliculaSalaDeCine.SalasDeCineId
                WHERE PeliculasId = Peliculas.Id) as CantidadCines,
                (
                Select count(*)
                FROM PeliculasActores
                where PeliculaId = Peliculas.Id) as CantidadActores
                FROM Peliculas
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Moneda",
                table: "SalasDeCines");

            migrationBuilder.Sql("DROP VIEW [dbo].[PeliculasConConteos]");
        }
    }
}
