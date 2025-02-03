using Configuration;

namespace Models;

public class Role : IRole
{
    public virtual Guid RoleId { get; set; }
    public enumRoles Roles { get; set; }
   

    
}