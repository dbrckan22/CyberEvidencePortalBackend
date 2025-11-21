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
    }
}
