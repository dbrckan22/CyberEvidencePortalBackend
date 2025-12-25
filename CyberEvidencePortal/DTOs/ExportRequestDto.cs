namespace CyberEvidencePortal.DTOs;

public class ExportRequestDto
{
    public int UserId { get; set; }

    public List<int> CategoryIds { get; set; } = new();

    public bool IncludeExpired { get; set; }

    public bool IncludeMetadata { get; set; }
}
