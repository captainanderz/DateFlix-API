using System;
using DateflixMVC.Models;
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
        public DbSet<Inquiries> Inquiries { get; set; }
        public DbSet<Blocks> Blocks { get; set; }
        public DbSet<Bans> Bans { get; set; }
        public DbSet<ActiveLogins> ActiveLogins { get; set; }
        public DbSet<DirectMessages> DirrectMessages { get; set; }
        public DbSet<Likes> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(x => x.ProfilePictures).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            base.OnModelCreating(modelBuilder);
        }
    }
}
