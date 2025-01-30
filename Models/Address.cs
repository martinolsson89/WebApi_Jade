using Configuration;

namespace Models;

public class Address : IAddress
{
    public virtual Guid AddressId { get; set;}
    public string City { get; set;}
    public string Country { get; set; }
    public bool Seeded { get; set; } = false;
}