using Models;
using Models.DTO;
namespace Services;

public interface IUserService
{
    Task<ResponseItemDto<IUser>> ReadUserAsync(Guid id);
    Task<ResponseItemDto<IUser>> CreateUserAsync(UserCuDto item);
    Task<ResponseItemDto<IUser>> UpdateUserAsync(UserCuDto item);
    Task<ResponseItemDto<IUser>> DeleteUserAsync(Guid id);
}
