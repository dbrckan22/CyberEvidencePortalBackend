using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CyberEvidencePortal.DTOs
{
    public class EvidenceUploadDto
    {
        [Required]
        public int ObligationId { get; set; }

        [Required]
        public int UserId { get; set; } 

        [Required]
        public string EvidenceType { get; set; }

        [Required]
        public string Title { get; set; }

        public IFormFile? File { get; set; }
        public string? LinkUrl { get; set; }
        public string? Note { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
