using Models;

public class AttractionFinancialDto
{
    public Guid AttractionId { get; set; }
    public FinancialRisk Risk { get; set; }
    public decimal Revenue { get; set; }
}