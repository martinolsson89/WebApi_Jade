using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Models;
using DbModels;
using DbContext;

namespace DbRepos;

public class RoleDbRepos
{
    private readonly ILogger<RoleDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public RoleDbRepos(ILogger<RoleDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    public async Task<Role> ReadRoleAsync(Guid roleId)
    {
        var role = await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleId == roleId);
        if (role == null)
        {
            _logger.LogInformation($"Role with ID {roleId} not found.");
            return null;
        }
        return role;
    }

    public async Task<Role> CreateRoleAsync(Role role)
    {
        _dbContext.Roles.Add(role);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"New role created with ID {role.RoleId}.");
        return role;
    }

    public async Task<Role> UpdateRoleAsync(Role role)
    {
        var existingRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleId == role.RoleId);
        if (existingRole == null)
        {
            _logger.LogError($"Role with ID {role.RoleId} not found.");
            return null;
        }

        existingRole.Name = role.Name;
        existingRole.Seeded = role.Seeded;
        _dbContext.Roles.Update(existingRole);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"Role with ID {role.RoleId} updated.");
        return existingRole;
    }

    public async Task<bool> DeleteRoleAsync(Guid roleId)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
        if (role == null)
        {
            _logger.LogError($"Role with ID {roleId} not found.");
            return false;
        }

        _dbContext.Roles.Remove(role);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"Role with ID {roleId} deleted.");
        return true;
    }
}
