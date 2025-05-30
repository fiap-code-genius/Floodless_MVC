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
            modelBuilder.Entity<RecursoEntity>()
                .Property(r => r.TipoRecurso)
                .HasConversion<string>();
        }

        public DbSet<RecursoEntity> Recurso { get; set; }
        public DbSet<VoluntarioEntity> Voluntario { get; set; }
    }
}
