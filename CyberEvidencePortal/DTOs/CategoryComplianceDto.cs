namespace CyberEvidencePortal.DTOs;

public class CategoryComplianceDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }

    public ComplianceSummaryDto Summary { get; set; }

    public List<ObligationStatusDto> Obligations { get; set; }
}


public class ComplianceSummaryDto
{
    public int TotalObligations { get; set; }
    public int CoveredObligations { get; set; }
    public double CoveragePercent { get; set; }

    public int Valid { get; set; }
    public int ExpiringSoon { get; set; }
    public int Expired { get; set; }
    public int Missing { get; set; }
}


