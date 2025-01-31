using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;


namespace Services;

public class UserServiceDb : IUserService
{
    private readonly UserDbRepos _userDbRepos;
    private readonly ILogger<UserServiceDb> _logger;

    public UserServiceDb(UserDbRepos userDbRepos, ILogger<UserServiceDb> logger)
    {
        _userDbRepos = userDbRepos;
        _logger = logger;
    }

    public Task<ResponseItemDto<IUser>> ReadUserAsync(Guid id) => _userDbRepos.ReadItemAsync(id);

    public Task<ResponseItemDto<IUser>> CreateUserAsync(UserCuDto item) => _userDbRepos.CreateItemAsync(item);

    public Task<ResponseItemDto<IUser>> UpdateUserAsync(UserCuDto item) => _userDbRepos.UpdateItemAsync(item);

    public Task<ResponseItemDto<IUser>> DeleteUserAsync(Guid id) => _userDbRepos.DeleteItemAsync(id);
}
