using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EntitiesLayer.Models;

namespace DataAccessLayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Meeting> Meetings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Meetings)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId);

        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost,1434;Database=MeetManagerDB;User Id=sa;Password=Password1;TrustServerCertificate=True;Encrypt=True");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
