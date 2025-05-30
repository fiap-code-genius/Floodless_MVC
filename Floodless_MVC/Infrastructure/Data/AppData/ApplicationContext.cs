using Floodless_MVC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Floodless_MVC.Infrastructure.Data.AppData
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recurso>()
                .Property(r => r.TipoRecurso)
                .HasConversion<string>();
        }

        public DbSet<Recurso> Recurso { get; set; }
        public DbSet<Voluntario> Voluntario { get; set; }
    }
}
