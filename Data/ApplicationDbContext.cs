using EFCore.Model;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {

        }


        public DbSet<Persona> Personas { get; set; }


    }
}
