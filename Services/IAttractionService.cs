using Models;
using Models.DTO;

namespace Services;

public interface IAttractionService {

    public Task<List<IAttraction>> ReadAsync();
    public Task<IAttraction> ReadItemAsync(Guid id);
}