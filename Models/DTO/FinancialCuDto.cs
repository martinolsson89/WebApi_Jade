

namespace Models.DTO;

public class FinancialCuDto 
{
    public Guid? FinancialId { get; set; }
    public FinancialRisk? Risk { get; set;}
    public string? Revenue { get; set; }

    public string Comments { get; set; }

    

    public Guid AttractionId { get; set; }

    public FinancialCuDto()
    {
        
    }

    public FinancialCuDto(IFinancial org)
    {
        FinancialId = org.FinancialId;
        Risk = org.Risk;
        Revenue = org.Revenue;
        Comments = org.Comments;
        AttractionId = org.Attraction.AttractionId;
    }
}