using Models;
namespace Models;

public enum FinancialRisk { VeryLow, Low, High, VeryHigh }

public interface IFinancial
{

    public Guid FinancialId { get; set;}
    
    public FinancialRisk? Risk { get; set; }
    public string? Revenue { get; set; }

    public string Comments { get; set; }

    public IAttraction Attraction { get; set; }

   

    // User prop here after
}