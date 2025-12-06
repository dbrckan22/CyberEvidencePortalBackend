namespace CyberEvidencePortal.DTOs
{
    public class CategoryWithObligationsDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<ObligationShortDto> Obligations { get; set; }
    }

    public class ObligationShortDto
    {
        public int ObligationId { get; set; }
        public string Title { get; set; }
    }
}
