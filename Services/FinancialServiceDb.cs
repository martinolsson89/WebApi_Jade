using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;

public class FinancialServiceDb : IFinancialService
{
    private readonly FinancialRepos _repo;
    private readonly ILogger<FinancialServiceDb> _logger;

   

    public FinancialServiceDb(ILogger<FinancialServiceDb> logger, FinancialRepos repo)
    {
        _repo = repo;
        _logger = logger;
       
    }

    public Task<ResponseItemDto<IFinancial>> AddFinancial(FinancialCuDto fin) => _repo.AddFinancial(fin);
    
}