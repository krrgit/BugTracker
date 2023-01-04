using BugTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Data
{
    public class AppDBContext : IdentityDbContext<Member>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
		public DbSet<Member> AppUsers { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectMember>()
                .HasKey(mp => new { mp.AppUserId, mp.ProjectId });

            modelBuilder.Entity<ProjectMember>()
                .HasOne(mp => mp.AppUser)
                .WithMany(m => m.MemberProjects)
                .HasForeignKey(mp => mp.AppUserId);

            modelBuilder.Entity<ProjectMember>()
                .HasOne(mp => mp.Project)
                .WithMany(p => p.ProjectMembers)
                .HasForeignKey(mp => mp.ProjectId);
        }
    }
}
