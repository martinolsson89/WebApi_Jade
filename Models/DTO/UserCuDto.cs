using System;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Xml.Linq;
using Microsoft.Extensions.Azure;


namespace Models.DTO;

public class UserCuDto
{
    public virtual Guid? UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public UserCuDto() { }

    public UserCuDto(IUser org)
    {
        UserId = org.UserId;
        Username = org.Username;
        Password = org.Password;
    }
}
