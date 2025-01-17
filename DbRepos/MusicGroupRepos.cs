using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;

using Models.DTO;
using Models;
using DbModels;
using DbContext;

namespace DbRepos;

public class MusicGroupDbRepos
{
    private readonly ILogger<MusicGroupDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public MusicGroupDbRepos(ILogger<MusicGroupDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    public async Task<List<IMusicGroup>> ReadAsync ()
    {
        var mg = await _dbContext.MusicGroups.OrderBy(mg=> mg.Name).ToListAsync<IMusicGroup>();
        return mg;
    } 
    public async Task<IMusicGroup> ReadItemAsync (Guid id)
    {
        var mg = await _dbContext.MusicGroups.Where(mg => mg.Id == id).FirstAsync();
        return mg;
    } 
}