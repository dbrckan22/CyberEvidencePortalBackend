using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberEvidencePortal.Models
{
    [Table("organizations")]
    public class Organization
    {
        [Key]
        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("oib")]
        [StringLength(11)]
        public string Oib { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<User> Users { get; set; }
    }
}
