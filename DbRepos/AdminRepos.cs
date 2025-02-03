using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using Models.DTO;
using DbModels;
using DbContext;
using Seido.Utilities.SeedGenerator;
using Microsoft.Identity.Client;

namespace DbRepos;

public class AdminDbRepos
{
    private readonly ILogger<AdminDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    #region contructors
    public AdminDbRepos(ILogger<AdminDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }
    #endregion

    public async Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync()
    {
        var info = new GstUsrInfoAllDto();
        info.Db = await _dbContext.InfoDbView.FirstAsync();
        info.Comments = await _dbContext.InfoCommentsView.ToListAsync();

        return new ResponseItemDto<GstUsrInfoAllDto>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = info
        };
    }

    public async Task<ResponseItemDto<GstUsrInfoAllDto>> SeedAsync(int nrOfItems)
    {
        try 
        {
            await RemoveSeedAsync(true);

        var rnd = new csSeedGenerator();
        var at = rnd.ItemsToList<AttractionDbM>(nrOfItems);
        var ad = rnd.ItemsToList<AddressDbM>(nrOfItems);

        var i = 0;
        
        foreach (var item in at){
            item.CategoryDbM = new CategoryDbM(rnd);
            item.AddressDbM = ad[i];
            i++;
        }

        _dbContext.Attractions.AddRange(at);
    
        await _dbContext.SaveChangesAsync();

            return await InfoAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in SeedAsync: {ex.Message}");
            _logger.LogError($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }
    
    public async Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded)
    {
            var parameters = new List<SqlParameter>();

            var retValue = new SqlParameter("retval", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var seededArg = new SqlParameter("seeded", seeded);

            parameters.Add(retValue);
            parameters.Add(seededArg);

            //there is no FromSqlRawAsync to I make one here
            var _query = await Task.Run(() =>
                _dbContext.InfoDbView.FromSqlRaw($"EXEC @retval = supusr.spDeleteAllAttractions @seeded",
                    parameters.ToArray()).AsEnumerable());

            //Execute the query and get the sp result set.
            //Although, I am not using this result set, but it shows how to get it
            GstUsrInfoDbDto result_set = _query.FirstOrDefault();

            //Check the return code
            int retCode = (int)retValue.Value;
            if (retCode != 0) throw new Exception("supusr.spDeleteAll return code error");

            return await InfoAsync();
    }
}
