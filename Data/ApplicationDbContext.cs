using Imarat_Shariah.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Imarat_Shariah.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<Siyajat> siyajats { get; set; }    
        public DbSet<Khula> khulas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Siyajat>().HasQueryFilter(s => s.DeletedDate == null);
            modelBuilder.Entity<Khula>().HasQueryFilter(s => s.DeletedDate == null);

            base.OnModelCreating(modelBuilder);
        }
    }
}
