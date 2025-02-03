using Models;
using Models.DTO;

namespace Services;

public interface IRoleService{
    public Task<ResponsePageDto<IRole>> ReadRolesAsync(bool flat, int pageNr, int pageSize);
    public Task<ResponseItemDto<IRole>> ReadRoleAsync(Guid id, bool flat);

    public Task<ResponseItemDto<IRole>> UpdateRoleAsync(RoleCuDto item);
}