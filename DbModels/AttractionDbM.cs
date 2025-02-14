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
    
    [JsonProperty(Order = 5)]
    public string RiskString { get => Risk.ToString(); set {} }

    [JsonProperty(Order = 6)]
    public string EncryptedRevenue { get; set; }

    [JsonProperty(Order = 7)]
    public string FormattedEncryptedRevenue { get; private set; } 

    [JsonIgnore]
    [NotMapped]
    public string OriginalRevenue { get; private set; } 


    [JsonIgnore]
    [Required]
    public CategoryDbM CategoryDbM { get; set; }

    [JsonIgnore]
    [Required]
    public AddressDbM AddressDbM { get; set; }

    [JsonIgnore]
    public List<CommentDbM> CommentsDbM { get; set; }

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
        Risk = org.Risk;
        OriginalRevenue = org.Revenue?.ToString();

        return this;
    }

    public AttractionDbM(AttractionCuDto org)
    {
        AttractionId = Guid.NewGuid();
        UpdateFromDTO(org);
    }

    public AttractionDbM EncryptFinancial(Func<string, string> encryptor, bool showEncrypt = false)
    {
        if (Revenue.HasValue)
        {
            EncryptedRevenue = encryptor(Revenue.Value.ToString());
            FormattedEncryptedRevenue = FormatAsDotted(Revenue.ToString()); 
             
                if (showEncrypt)
                {
                    OriginalRevenue = EncryptedRevenue;
                    
                }
            
            
        }

        return this;
    }

   
  public string GetDecryptedRevenue(Func<string, string> decryptor)
    {
    if (!string.IsNullOrEmpty(EncryptedRevenue))
    {
        OriginalRevenue = decryptor(EncryptedRevenue);
        System.Console.WriteLine(OriginalRevenue);
    }

        return OriginalRevenue;
    }


    
    private string FormatAsDotted(string encryptedValue) =>  $"{encryptedValue[0]}••••••••••{encryptedValue[encryptedValue.Length - 1]}";


    
}


