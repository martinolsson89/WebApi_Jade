using Models;
using Models.DTO;

namespace Services;

public interface IAdminService {

    public Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync();
    public Task<ResponseItemDto<GstUsrInfoAllDto>> SeedAsync(int nrAttractions, int nrAdresses);
    public Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded);
    public Task<UsrInfoDto> SeedUsersAsync(int nrOfUsers, int nrOfSuperUsers, int nrOfSysAdmin);
}