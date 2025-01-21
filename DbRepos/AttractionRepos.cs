using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;

using Models.DTO;
using Models;
using DbModels;
using DbContext;

namespace DbRepos;

public class AttractionDbRepos
{
    private readonly ILogger<AttractionDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public AttractionDbRepos(ILogger<AttractionDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    public async Task<List<IAttraction>> ReadAsync ()
    {
        var at = await _dbContext.Attractions.ToListAsync<IAttraction>();
        return at;
    } 
    public async Task<IAttraction> ReadItemAsync (Guid id)
    {
        var at = await _dbContext.Attractions.Where(at => at.AttractionId == id).FirstAsync();
        return at;
    } 
}