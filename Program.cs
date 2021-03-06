using EFCore.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    //Opcion utilizada para manejo de referencias ciclicas.
    .AddJsonOptions(opciones => opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Conexion a la DB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
{
    //Para utilizar geolocalizacion.
    opciones.UseSqlServer(connectionString, sqlServer => sqlServer.UseNetTopologySuite());

    //Para comportamiento por defecto con AsNoTracking por defecto.
    opciones.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    //Con esta seccion se habilita el lazyloading + la palabra "virtual" en la propiedades de navegacion de las entidades
    //opciones.UseLazyLoadingProxies();
});


//Habilitando el uso de AutoMapper
builder.Services.AddAutoMapper(typeof(Program));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
