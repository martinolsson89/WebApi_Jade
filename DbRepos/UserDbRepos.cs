using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using Models;
using DbModels;
using DbContext;

namespace DbRepos;

public class UserDbRepos
{
    private readonly ILogger<UserDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public UserDbRepos(ILogger<UserDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    public async Task<ResponseItemDto<IUser>> ReadItemAsync(Guid id)
    {
        var resp = await _dbContext.Users.AsNoTracking()
            .Where(i => i.UserId == id)
            .FirstOrDefaultAsync<IUser>();

        return new ResponseItemDto<IUser>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = resp
        };
    }

    public async Task<ResponseItemDto<IUser>> CreateItemAsync(UserCuDto itemDto)
    {
        if (itemDto.UserId != null)
            throw new ArgumentException($"{nameof(itemDto.UserId)} must be null when creating a new object");

        var item = new UserDbM(itemDto);

        _dbContext.Add(item);

        await _dbContext.SaveChangesAsync();

        return await ReadItemAsync(item.UserId);
    }

    public async Task<ResponseItemDto<IUser>> UpdateItemAsync(UserCuDto itemDto)
    {
        var query = _dbContext.Users.Where(u => u.UserId == itemDto.UserId);
        var item = await query.FirstOrDefaultAsync<UserDbM>();

        if (item == null) throw new ArgumentException($"User {itemDto.UserId} is not existing");

        item.UpdateFromDTO(itemDto);

        _dbContext.Update(item);

        await _dbContext.SaveChangesAsync();

        return await ReadItemAsync(item.UserId);
    }

    public async Task<ResponseItemDto<IUser>> DeleteItemAsync(Guid id)
    {
        var query = _dbContext.Users.Where(u => u.UserId == id);
        var item = await query.FirstOrDefaultAsync();

        if (item == null) throw new ArgumentException($"User: {id} is not existing");

        _dbContext.Remove(item);

        await _dbContext.SaveChangesAsync();

        return new ResponseItemDto<IUser>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = item
        };
    }
}
