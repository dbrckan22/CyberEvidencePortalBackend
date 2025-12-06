namespace CyberEvidencePortal.DTOs
{
    public class ObligationDto
    {
        public int ObligationId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string? LegalReference { get; set; }
        public int? ReviewIntervalMonths { get; set; }
        public bool IsApplicable { get; set; }
        public string? Justification { get; set; }
    }
}
