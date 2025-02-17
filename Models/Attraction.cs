using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Configuration;
using Newtonsoft.Json;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class Attraction : IAttraction, ISeed<Attraction>
{
        [JsonProperty(Order = 1)]
        public virtual Guid AttractionId { get; set; }

        [JsonProperty(Order = 2)]
        public string AttractionTitle { get; set; }

        [JsonProperty(Order = 3)]
        public string Description { get; set; }

        [JsonProperty(Order = 4)]
        public bool Seeded { get; set; } = false;

        [JsonProperty(Order = 8)]
        public virtual ICategory Category { get; set; }

        [JsonProperty(Order = 9)]
        public virtual IAddress Address { get; set; }

        [JsonProperty(Order = 10)]
        public virtual List<IComment>? Comments { get; set; } = null;

        
    public virtual IFinancial Financial { get; set; }

    public virtual Attraction Seed (csSeedGenerator seeder)
    {
        AttractionId = Guid.NewGuid();
        AttractionTitle = seeder.AttractionTitle;
        
        Description = seeder.LatinSentence;
        Seeded = true;

        return this;
    }
}
