namespace Models;

public interface IAddress
{
    public Guid AddressId { get; set; }
    string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    //Navigation properties
    public List<IAttraction> Attractions { get; set; }
}