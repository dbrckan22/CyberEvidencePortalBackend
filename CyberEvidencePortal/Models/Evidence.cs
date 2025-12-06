using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CyberEvidencePortal.Models
{
    [Table("evidence")]
    public class Evidence
    {
        [Key]
        [Column("evidence_id")]
        public int EvidenceId { get; set; }

        [Required]
        [Column("obligation_id")]
        public int ObligationId { get; set; }

        [Required]
        [Column("added_by")]
        public int AddedBy { get; set; }

        [Required]
        [Column("evidence_type")]
        public string EvidenceType { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Column("file_path")]
        public string? FilePath { get; set; }

        [Column("link_url")]
        public string? LinkUrl { get; set; }

        [Column("note")]
        public string? Note { get; set; }

        [Column("valid_from")]
        public DateTime? ValidFrom { get; set; }

        [Column("valid_until")]
        public DateTime? ValidUntil { get; set; }

        [Required]
        [Column("status")]
        public string Status { get; set; } = "Active";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [JsonIgnore]
        public Obligation Obligation { get; set; }

        [ForeignKey(nameof(AddedBy))]
        [JsonIgnore]
        public User User { get; set; }


    }
}
