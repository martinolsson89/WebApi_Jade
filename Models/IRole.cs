using Models;

public enum enumRoles {gstusr, usr, supusr, sysadmin}

public interface IRole{
    public Guid RoleId { get; set; }
    public enumRoles Roles{ get; set; }

   public List<IUser> Users { get; set; }

    // User prop here after
}