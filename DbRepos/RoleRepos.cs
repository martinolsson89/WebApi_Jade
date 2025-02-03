using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;

using Models.DTO;
using Models;
using DbModels;
using DbContext;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DbRepos;

public class RoleDbRepos
{
    private readonly ILogger<RoleDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public RoleDbRepos(ILogger<RoleDbRepos> logger, MainDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;

    }

    public async Task<ResponsePageDto<IRole>> ReadRolesAsync(bool flat, int pageNumber, int pageSize)
    {
        IQueryable<RoleDbM> query = _dbContext.Roles.AsNoTracking();
        // Kör en include users sen if flat

        return new ResponsePageDto<IRole>{
            DbConnectionKeyUsed = _dbContext.dbConnection,
            DbItemsCount = await query.CountAsync(),

            PageItems = await query 

            .Skip(pageNumber * pageSize)
                .Take(pageSize)

                .ToListAsync<IRole>(),

                PageNr = pageNumber,
                PageSize = pageSize

        };
    
    }

    public async Task<ResponseItemDto<IRole>> ReadItemAsync(Guid id, bool flat){
        IQueryable<RoleDbM> query = _dbContext.Roles.AsNoTracking()
            .Where(x => x.RoleId == id);

            //Flat logik här sen

            var item = await query.FirstOrDefaultAsync<IRole>();
            if (item == null)
            throw new ArgumentNullException($"Id number: {id} not found");

            return new ResponseItemDto<IRole> 
            {
                DbConnectionKeyUsed = _dbContext.dbConnection,
                Item = item
            };
    }



    public async Task<ResponseItemDto<IRole>> UpdateItemAsync(RoleCuDto itemDto)
    {
        var query = _dbContext.Roles.Where(r => r.RoleId == itemDto.RoleId);

        var item = await query.FirstOrDefaultAsync<RoleDbM>();

        if (item == null) throw new ArgumentException($"Item {itemDto.RoleId} is not existing");

        item.UpdateFromDto(itemDto); // Denna ska updatera users också

        _dbContext.Update(item);

        await _dbContext.SaveChangesAsync();

        return await ReadItemAsync(item.RoleId, true);
    }



}