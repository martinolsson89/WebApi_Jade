using Models;
using Models.DTO;

namespace Services;

public interface IMusicGroupService {

    public Task<List<IMusicGroup>> ReadAsync();
    public Task<IMusicGroup> ReadItemAsync(Guid id);
}