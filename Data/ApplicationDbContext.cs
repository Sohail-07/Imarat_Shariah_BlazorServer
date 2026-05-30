using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Imarat_Shariah.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<Siyajat> siyajats { get; set; }    
        public DbSet<Khula> khulas { get; set; }
        public DbSet<LogEntry> logEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Siyajat>().HasQueryFilter(s => s.DeletedDate == null);
            modelBuilder.Entity<Khula>().HasQueryFilter(s => s.DeletedDate == null);

            base.OnModelCreating(modelBuilder);
        }
    }
}
