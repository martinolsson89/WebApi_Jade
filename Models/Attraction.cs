using Configuration;

namespace Models;

public class Attraction : IAttraction
{
    public virtual Guid AttractionId { get; set; }
    public string Description { get; set; }
    public bool Seeded { get; set; }
}