using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Models;
using Models.DTO;
using Seido.Utilities.SeedGenerator;

namespace DbModels;

[Table("Financials", Schema = "supusr")]
public class FinancialDbM : Financial, ISeed<FinancialDbM>
{
    [Key]
    public override Guid FinancialId { get; set; }

    [NotMapped]
    [JsonIgnore]
    public override IAttraction Attraction { get => AttractionDbM; set => throw new NotImplementedException(); }

    
    [JsonIgnore]
    public virtual Guid AttractionId { get; set; }
  

    [JsonIgnore]
    [Required]
    [ForeignKey ("AttractionId")]
    
    public AttractionDbM  AttractionDbM { get; set; } = null;

    
    public FinancialDbM UpdateFromDto(FinancialCuDto org)
    {
        if (org == null) return null;

        Risk = org.Risk;
        Revenue = org.Revenue;
        Comments = org.Comments;

        return this;

    }

    public FinancialDbM(FinancialCuDto org)
    {
        FinancialId = Guid.NewGuid();
        UpdateFromDto(org);
    }
   

    public FinancialDbM()
    {
        
    }

    FinancialDbM ISeed<FinancialDbM>.Seed(csSeedGenerator seedGenerator)
    {
        base.Seed (seedGenerator);
        return this;
    }
}