using Models;

public enum RoleName {GuidUser, user, Admin, SuperUser}

public interface IRole {

    public Guid RoleId { get; set; }

    public RoleName Name { get; set; }

    //public List<IAttraction> Attractions { get; set; }

}