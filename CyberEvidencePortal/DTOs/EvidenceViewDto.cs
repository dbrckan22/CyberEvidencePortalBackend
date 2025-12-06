namespace CyberEvidencePortal.DTOs
{
    public class EvidenceViewDto
    {
        public int EvidenceId { get; set; }
        public string Title { get; set; }
        public string EvidenceType { get; set; }
        public string? FileName { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string AddedByUser { get; set; }
        public string? Note { get; set; }
    }
}
