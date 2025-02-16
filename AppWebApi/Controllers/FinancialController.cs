using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
            Policy = null, Roles = "sysadmin")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FinancialController : Controller
    {
        readonly IFinancialService _financialService;
        readonly ILogger<FinancialController> _logger;

        public FinancialController(IFinancialService financialService, ILogger<FinancialController> logger)
        {
            _logger = logger;
            _financialService = financialService;
        }

        [HttpPost()]
        [ProducesResponseType(200, Type = typeof(ResponsePageDto<IComment>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> PostFinancial(FinancialCuDto fin)
        {
            try
            {
                var resp = await _financialService.AddFinancial(fin);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                    return BadRequest(ex.Message);
            }
        }
    }
}