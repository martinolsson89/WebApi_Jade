using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Models;
using Models.DTO;
using Seido.Utilities.SeedGenerator;

namespace DbModels;

[Table("Attractions", Schema = "supusr")]
public class AttractionDbM : Attraction, ISeed<AttractionDbM>
{   
    [Key]
    public override Guid AttractionId { get; set; }

    [NotMapped]
    [JsonProperty(Order = 8)]
    public override ICategory Category { get => CategoryDbM; set => throw new NotImplementedException(); }

    [NotMapped]
    [JsonProperty(Order = 10)]
    public override List<IComment>? Comments { get => CommentsDbM?.ToList<IComment>() ?? null; set => throw new NotImplementedException(); }

    [NotMapped]
    [JsonProperty(Order = 9)]
    public override IAddress Address { get => AddressDbM; set => throw new NotImplementedException(); }

    [NotMapped]
    [JsonProperty(Order = 10)]
    public override IFinancial Financial { get => FinancialDbM; set => throw new NotImplementedException(); }

    [JsonIgnore]
    [Required]
    public CategoryDbM CategoryDbM { get; set; }

    [JsonIgnore]
    [Required]
    public AddressDbM AddressDbM { get; set; }

    [JsonIgnore]
    public List<CommentDbM> CommentsDbM { get; set; }

     [JsonIgnore]
    public FinancialDbM FinancialDbM { get; set; }

    public AttractionDbM(){ }

    public override AttractionDbM Seed(csSeedGenerator rnd)
    {
        base.Seed(rnd);
        return this;
    }

    public AttractionDbM UpdateFromDTO(AttractionCuDto org)
    {
        if (org == null) return null;

        AttractionTitle = org.AttractionTitle;
        Description = org.Description;
       

        return this;
    }

    public AttractionDbM(AttractionCuDto org)
    {
        AttractionId = Guid.NewGuid();
        UpdateFromDTO(org);
    }

  


    
}


