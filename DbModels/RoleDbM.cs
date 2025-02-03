using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Models.DTO;
using Seido.Utilities.SeedGenerator;

namespace DbModels;

public class RoleDbM : Role
{
    [Key]
    public override Guid RoleId { get; set; }

    public string Rolekind { get => Roles.ToString(); set {} }

    // Lägg till en lista av users sen

    

    public RoleDbM()
    {
        
    }

    public RoleDbM UpdateFromDto(RoleCuDto dto){
        RoleId = dto.RoleId;

        return this;
    }

    
}