using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;


public class MusicGroupServiceDb : IMusicGroupService {

    private readonly MusicGroupDbRepos _musicgroupRepo;
    private readonly ILogger<MusicGroupServiceDb> _logger;    
    
    public MusicGroupServiceDb(MusicGroupDbRepos musicgroupRepo, ILogger<MusicGroupServiceDb> logger)
    {
        _musicgroupRepo = musicgroupRepo;
        _logger = logger;
    }

    public Task<List<IMusicGroup>> ReadAsync() => _musicgroupRepo.ReadAsync();
    public Task<IMusicGroup> ReadItemAsync(Guid id) => _musicgroupRepo.ReadItemAsync(id);
}