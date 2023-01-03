using BugTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Data
{
    public class AppDBContext : IdentityDbContext<AppUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUserProject>()
                .HasKey(mp => new { mp.AppUserId, mp.ProjectId });

            modelBuilder.Entity<AppUserProject>()
                .HasOne(mp => mp.AppUser)
                .WithMany(m => m.MemberProjects)
                .HasForeignKey(mp => mp.AppUserId);

            modelBuilder.Entity<AppUserProject>()
                .HasOne(mp => mp.Project)
                .WithMany(p => p.ProjectMembers)
                .HasForeignKey(mp => mp.ProjectId);
        }
    }
}
