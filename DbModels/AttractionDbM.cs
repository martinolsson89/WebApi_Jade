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

    public AttractionDbM Seed(csSeedGenerator rnd)
    {
        this.AttractionId = Guid.NewGuid();
        this.Description = rnd.LatinSentence;

        return this;
    }
}