using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;

public class AttractionServiceDb : IAttractionService
{
    private readonly AttractionDbRepos _attractionDbRepos;
    private readonly ILogger<AttractionServiceDb> _logger;    
    
    public AttractionServiceDb(AttractionDbRepos attractionDbRepos, ILogger<AttractionServiceDb> logger)
    {
        _attractionDbRepos = attractionDbRepos;
        _logger = logger;
    }

    public Task<ResponseItemDto<IAttraction>> ReadAttractionAsync(Guid id, bool flat) => _attractionDbRepos.ReadItemAsync(id, flat);
    

    public Task<ResponsePageDto<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _attractionDbRepos.ReadItemsAsync(seeded, flat, filter, pageNumber, pageSize);
    
}