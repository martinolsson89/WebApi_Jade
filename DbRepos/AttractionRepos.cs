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

    public async Task<ResponseItemDto<IAttraction>> ReadItemAsync (Guid id, bool flat)
    {
        IQueryable<AttractionDbM> query = _dbContext.Attractions.AsNoTracking()
            .Where(i => i.AttractionId == id);

            var resp =  await query.FirstOrDefaultAsync<IAttraction>();
            return new ResponseItemDto<IAttraction>()
            {
                DbConnectionKeyUsed = _dbContext.dbConnection,
                Item = resp
            };

        // var at = await _dbContext.Attractions.Where(at => at.AttractionId == id).FirstAsync();
        // return at;
    }

    public async Task<ResponsePageDto<IAttraction>> ReadItemsAsync (bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        filter ??= "";

        IQueryable<AttractionDbM> query = _dbContext.Attractions.AsNoTracking();

        return new ResponsePageDto<IAttraction>
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            DbItemsCount = await query

             .Where(i => (i.Seeded == seeded) && 
                    i.Description.ToLower().Contains(filter)).CountAsync(),

             PageItems = await query

            //Adding filter functionality
            .Where(i => (i.Seeded == seeded) && 
                    i.Description.ToLower().Contains(filter))
                        

            //Adding paging
            .Skip(pageNumber * pageSize)
            .Take(pageSize)

            .ToListAsync<IAttraction>(),

            PageNr = pageNumber,
            PageSize = pageSize


        };

        // var at = await _dbContext.Attractions.ToListAsync<IAttraction>();
        // return at;
    } 
     
}