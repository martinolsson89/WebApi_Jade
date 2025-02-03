using Models;
using Seido.Utilities.SeedGenerator;

namespace Models;
public class Role : IRole, ISeed<Role>
{
    public Guid RoleId { get; set;}
    public RoleName Name { get;set;}
    public bool Seeded { get;set;}

    public Role Seed(csSeedGenerator seedGenerator)
    {
        throw new NotImplementedException();
    }
}