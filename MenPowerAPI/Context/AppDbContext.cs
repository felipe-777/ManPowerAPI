using MenPowerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MenPowerAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Timekeeping> Timekeeping { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Timekeeping>().ToTable("timekeeping");
        }
    }
}
