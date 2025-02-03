using System.Runtime.CompilerServices;
using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;

public class RoleServiceDb : IRoleService
{
    private readonly ILogger<RoleServiceDb> _logger;
    private readonly RoleDbRepos _RoleDbRepos;

    public RoleServiceDb(RoleDbRepos RoleDbRepos, ILogger<RoleServiceDb> logger)
    {
        _logger = logger;
        _RoleDbRepos = RoleDbRepos;
    }

    public Task<ResponseItemDto<IRole>> ReadRoleAsync(Guid id, bool flat) => _RoleDbRepos.ReadItemAsync(id, flat);

    

    public Task<ResponsePageDto<IRole>> ReadRolesAsync(bool flat, int pageNr, int pageSize) => _RoleDbRepos.ReadRolesAsync(flat, pageNr, pageSize);
    

    public Task<ResponseItemDto<IRole>> UpdateRoleAsync(RoleCuDto item) => _RoleDbRepos.UpdateItemAsync(item);
    
}
