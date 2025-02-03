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

    [NotMapped] // Not mapped betyder strunta i relationen
    public override ICategory Category { get => CategoryDbM; set => throw new NotImplementedException(); }

    [JsonIgnore]
    [Required]
    public CategoryDbM CategoryDbM { get; set; }
    public AttractionDbM(AttractionCuDto org)
    {
        AttractionId = new Guid();
        UpdateFromDTO(org);
    }

    [NotMapped]
    public override IAddress Address { get => AddressDbM; set => throw new NotImplementedException(); }

    [JsonIgnore]
    [Required]
    public AddressDbM AddressDbM { get; set;}

    public AttractionDbM Seed(csSeedGenerator rnd)
    {
        this.AttractionId = Guid.NewGuid();
        this.Description = rnd.LatinSentence;

        return this;
    }

    public AttractionDbM(){ }

    public AttractionDbM UpdateFromDTO(AttractionCuDto org)
    {
        Description = org.Description;
        
        return this;
    }
}