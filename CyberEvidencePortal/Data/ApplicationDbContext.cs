using CyberEvidencePortal.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberEvidencePortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Obligation> Obligations { get; set; }
        public DbSet<Evidence> Evidence { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Evidence>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.AddedBy)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }


    }
}
