using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using Models.DTO;
using DbModels;
using DbContext;
using Configuration;

namespace DbRepos;

public class FinancialRepos
{
    private readonly MainDbContext _context;
    private readonly ILogger<FinancialRepos> _logger;
    private Encryptions _encryptions;

    public FinancialRepos(ILogger<FinancialRepos> logger, MainDbContext context, Encryptions encryptions)
    {
        _logger = logger;
        _context = context;
        _encryptions = encryptions;
    }

   public async Task<ResponseItemDto<IFinancial>> AddFinancial(FinancialCuDto fin)
{
    try
    {
        _logger.LogInformation($"Processing Financial for AttractionId: {fin.AttractionId}");

        
        var attraction = await _context.Attractions
            .Include(a => a.FinancialDbM) 
            .FirstOrDefaultAsync(a => a.AttractionId == fin.AttractionId);

        if (attraction == null)
        {
            _logger.LogError($" Attraction with ID {fin.AttractionId} not found.");
            throw new Exception("Attraction doesn't exist.");
        }

        
        if (attraction.FinancialDbM != null)
        {
            _logger.LogInformation($"Found existing FinancialDbM for Attraction {fin.AttractionId}, replacing it...");

            
            _context.Financials.Remove(attraction.FinancialDbM);
            await _context.SaveChangesAsync(); 
        }

        
        var newFinancial = new FinancialDbM(fin)
        {
            AttractionDbM = attraction 
        };

        newFinancial.EnryptAndObfuscate(_encryptions.AesEncryptToBase64);

        _logger.LogInformation($"Created new FinancialDbM for Attraction {fin.AttractionId}");
        attraction.FinancialDbM = newFinancial; 
        _context.Financials.Add(newFinancial);
        await _context.SaveChangesAsync();

        return new ResponseItemDto<IFinancial>
        {
            DbConnectionKeyUsed = _context.dbConnection,
            Item = newFinancial
        };
    }
    catch (Exception ex)
    {
        _logger.LogError($"Funkar inte: {ex.Message}");
        throw new Exception("An error occurred while adding financial data.", ex);
    }
}


}