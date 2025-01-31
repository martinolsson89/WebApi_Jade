using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Models;
using Models.DTO;

namespace DbModels;

[Table("Users", Schema = "supusr")]
public class UserDbM : User
{
    [Key]
    public override Guid UserId { get; set; }

    public UserDbM(UserCuDto org)
    {
        UserId = new Guid();
        UpdateFromDTO(org);
    }

    public UserDbM() { }

    public UserDbM UpdateFromDTO(UserCuDto org)
    {
        Username = org.Username;
        Password = org.Password;
        
        return this;
    }
}
