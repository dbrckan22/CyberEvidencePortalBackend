namespace CyberEvidencePortal.DTOs;

public class ObligationStatusDto
{
    public int ObligationId { get; set; }
    public string Title { get; set; }
    public ComplianceStatus Status { get; set; }
    public int EvidenceCount { get; set; }
}

public enum ComplianceStatus
{
    Missing,
    Expired,
    ExpiringSoon,
    Valid
}


