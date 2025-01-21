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

    public Task<List<IAttraction>> ReadAsync() => _attractionDbRepos.ReadAsync();
    public Task<IAttraction> ReadItemAsync(Guid id) => _attractionDbRepos.ReadItemAsync(id);

}