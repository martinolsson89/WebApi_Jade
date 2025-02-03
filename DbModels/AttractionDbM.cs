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

    [NotMapped]
    public override List<IComment> Comments { get => CommentsDbM?.ToList<IComment>(); set => throw new NotImplementedException(); }

    [JsonIgnore]
    public List<CommentDbM> CommentsDbM { get; set; }

    public override AttractionDbM Seed(csSeedGenerator rnd)
    {
        base.Seed (rnd);
        return this;
    }

    public AttractionDbM(){ }

    public AttractionDbM UpdateFromDTO(AttractionCuDto org)
    {
        if (org == null) return null;

        Description = org.Description;
        
        return this;
    }

    public AttractionDbM(){ }
    public AttractionDbM(AttractionCuDto org)
    {
        AttractionId = new Guid();
        UpdateFromDTO(org);
    }
}
