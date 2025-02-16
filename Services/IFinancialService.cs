using System;
using Models;
using Models.DTO;

namespace Services;

public interface IFinancialService
{
    public Task<ResponseItemDto<IFinancial>> AddFinancial(FinancialCuDto fin);
}
