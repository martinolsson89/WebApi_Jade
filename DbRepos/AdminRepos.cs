using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using Models.DTO;
using DbModels;
using DbContext;
using Seido.Utilities.SeedGenerator;

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

            // Create categories first
            var categories = rnd.ItemsToList<CategoryDbM>(Math.Max(3, nrOfItems / 10));
            _dbContext.Catgeories.AddRange(categories);
            await _dbContext.SaveChangesAsync();

            // Create attractions with references to categories
            var attractions = rnd.ItemsToList<AttractionDbM>(nrOfItems);
            foreach (var attraction in attractions)
            {
                attraction.Seeded = true;
                attraction.CategoryDbM = categories[rnd.Next(0, categories.Count)];
            }
            _dbContext.Attractions.AddRange(attractions);
            await _dbContext.SaveChangesAsync();

            // Create comments with references to attractions
            var comments = new List<CommentDbM>();
            foreach (var attraction in attractions)
            {
                var attractionComments = rnd.ItemsToList<CommentDbM>(rnd.Next(0, 21));
                foreach (var comment in attractionComments)
                {
                    comment.Seeded = true;
                    comment.AttractionDbM = attraction;
                    comments.Add(comment);
                }
            }
            _dbContext.Comments.AddRange(comments);
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
