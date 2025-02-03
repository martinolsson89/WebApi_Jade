using Configuration;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class Attraction : IAttraction, ISeed<Attraction>
{
    public virtual Guid AttractionId { get; set; }
    public string Description { get; set; }

    public virtual ICategory Category { get; set; }

    public virtual IAddress Address{ get; set; }
    public virtual List<IComment> Comments { get; set; }
    public bool Seeded { get; set; }

    public virtual Attraction Seed (csSeedGenerator seeder)
    {
        AttractionId = Guid.NewGuid();
        Description = seeder.LatinSentence;
        Seeded = true;

        return this;
    }
}
