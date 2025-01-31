using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;

using Models.DTO;
using Models;
using DbModels;
using DbContext;
using Microsoft.Identity.Client;

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

            if (!flat){
                query = query.Include(a => a.CategoryDbM)
                .Include(x => x.CommentsDbM);   
            }
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

         if (!flat){
             query = query
             .Include(a => a.CategoryDbM)
             .Include(x => x.CommentsDbM);
              
            }

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

    public async Task<ResponseItemDto<IAttraction>> DeleteItemAsync(Guid id)
    {
        var query = _dbContext.Attractions
        .Where(a => a.AttractionId == id);
        var item = await query.FirstOrDefaultAsync();

        if(item == null) throw new ArgumentException($"Item: {id} is not existing");

        await _dbContext.SaveChangesAsync();

        return new ResponseItemDto<IAttraction> 
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = item
        };
    }

    public async Task<ResponseItemDto<IAttraction>> DeleteAttraction(Guid id)
    {
        var query = _dbContext.Attractions.Where(a => a.AttractionId == id);
        var item = await query.FirstOrDefaultAsync<AttractionDbM>();

        if(item == null) throw new ArgumentException($"No object linked to id: {id}");

        _dbContext.Remove(item);

        await _dbContext.SaveChangesAsync();

        return new ResponseItemDto<IAttraction>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = item
        };
    }

    public async Task<ResponseItemDto<IAttraction>> CreateItemAsync(AttractionCuDto itemDto)
    {
        if (itemDto.AttractionId != null) 
          throw new ArgumentException($"{nameof(itemDto.AttractionId)} must be null when creating a new object");

          var item = new AttractionDbM(itemDto);

          await UpdateNavigationProp(itemDto, item);

          _dbContext.Add(item);

          await _dbContext.SaveChangesAsync();

          return await ReadItemAsync(item.AttractionId, true);
    }

    public async Task<ResponseItemDto<IAttraction>> UpdateItemAsync(AttractionCuDto itemDto)
    {
        var query = _dbContext.Attractions.Where(a => a.AttractionId != itemDto.AttractionId);

        var item = await query.FirstOrDefaultAsync<AttractionDbM>();

        if (item == null) throw new ArgumentException($"Item {itemDto.AttractionId} is not existing");

        item.UpdateFromDTO(itemDto);

        await UpdateNavigationProp(itemDto, item);

        _dbContext.Update(item);

        await _dbContext.SaveChangesAsync();

        return await ReadItemAsync(item.AttractionId, true);
    }

    public async Task UpdateNavigationProp(AttractionCuDto itemDto, AttractionDbM item)
    {
        var updatedCat = await _dbContext.Catgeories.Where(a => a.CategoryId == itemDto.CategoryId).FirstOrDefaultAsync();

        if(updatedCat == null) throw new ArgumentException($"This id: {itemDto.CategoryId} does not exist");

        item.CategoryDbM = updatedCat;
    }
     
}