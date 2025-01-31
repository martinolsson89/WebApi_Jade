using Configuration;

namespace Models;

public class User : IUser
{
    public virtual Guid UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
