using DateflixMVC.Models.Profile;
using Microsoft.EntityFrameworkCore;

namespace DateflixMVC.Helpers
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext(DbContextOptions options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
