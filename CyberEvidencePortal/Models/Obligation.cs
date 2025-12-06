using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CyberEvidencePortal.Models
{
    [Table("obligations")]
    public class Obligation
    {
        [Key]
        [Column("obligation_id")]
        public int ObligationId { get; set; }

        [Required]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Required]
        [Column("code")]
        public string Code { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Column("description", TypeName = "text")]
        public string? Description { get; set; }

        [Column("legal_reference")]
        public string? LegalReference { get; set; }

        [Column("review_interval_months")]
        public int? ReviewIntervalMonths { get; set; }

        [Required]
        [Column("is_applicable")]
        public bool IsApplicable { get; set; }

        [Column("justification", TypeName = "varchar(max)")]
        public string? Justification { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }

        [JsonIgnore]
        public ICollection<Evidence> EvidenceItems { get; set; }
    }
}
