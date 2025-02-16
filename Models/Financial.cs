using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Seido.Utilities.SeedGenerator;

namespace Models;
public class Financial : IFinancial, ISeed<Financial>
{

    public virtual Guid FinancialId { get; set ; }
    [EnumDataType(typeof(FinancialRisk), ErrorMessage = "Outside Enum value")]
    public FinancialRisk? Risk { get; set;}
    public string? Revenue { get; set; }

    [JsonIgnore] // Prevents circular serialization issues
    public string EncryptedRisk { get; set; }
    public bool Seeded { get; set; } = false;
    [MaxLength(2000)]
    public string Comments { get; set; }


    public string strRisk 
    { 
        get => Risk.HasValue ? Risk.ToString() : "Unknown"; // ✅ Shows decrypted risk
    }


    public virtual IAttraction Attraction { get; set; }

    [JsonIgnore]
    public string EnryptedToken {get; set; }
    

    public Financial Seed(csSeedGenerator seedGenerator)
    {
        FinancialId = Guid.NewGuid();
        Risk = seedGenerator.FromEnum<FinancialRisk>();    
        Revenue = $"{seedGenerator.NextDecimal(100000, 1000000).ToString()} Kr";    
        Comments = seedGenerator.FromString(seedGenerator.LatinParagraph);


        return this;

    }

    public Financial EnryptAndObfuscate(Func<Financial, string> encryptor)
    {
        this.EnryptedToken = encryptor(this);

        this.Comments = Regex.Replace(Comments, "(?<=.{1}).", "*");

        
        this.Revenue = Regex.Replace(Revenue, ".", "*");

        /*
        if (Risk.HasValue)
        {
            this.EncryptedRisk = encryptor(Risk.ToString()); 
        }
*/
        return this;
    }


     public Financial Decrypt (Func<string, Financial> decryptor)
    {
        return decryptor(this.EnryptedToken);
    }

    public void EncryptRisk(Func<string, string> encryptor)
    {
        if (Risk.HasValue)
        {
            EncryptedRisk = encryptor(Risk.ToString()); // ✅ Encrypt risk before saving
        }
    }

    public void DecryptRisk(Func<string, string> decryptor)
    {
        if (!string.IsNullOrEmpty(EncryptedRisk))
        {
            var decrypted = decryptor(EncryptedRisk);
            Risk = Enum.TryParse(decrypted, out FinancialRisk riskEnum) ? riskEnum : null; // ✅ Decrypt risk
        }
    }



}