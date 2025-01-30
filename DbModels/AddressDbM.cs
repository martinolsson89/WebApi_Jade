using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Models.DTO;
using Seido.Utilities.SeedGenerator;

namespace DbModels;

[Table("Addresses", Schema = "supusr")]
public class AddressDbM : Address, ISeed<AddressDbM>
{   
    [Key]
    public override Guid AddressId { get; set; }

    public AddressDbM(){ }
    

    public AddressDbM Seed(csSeedGenerator rnd)
    {
        this.AddressId = Guid.NewGuid();
        this.Country = rnd.Country;
        this.City = rnd.City(Country);


        return this;
    }
    
}