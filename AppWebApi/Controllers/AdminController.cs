using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Models.DTO;
using Services;
using Configuration;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
   

#if !DEBUG    
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Policy = null, Roles = "sysadmin")] // Den kollar ifall claimsen innehåller en Role med innehållet sysadmin
#endif
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminController : Controller
    {
        readonly DatabaseConnections _dbConnections;
        readonly IAdminService _adminService;
        readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminService, ILogger<AdminController> logger, DatabaseConnections dbConnections)
        {
            _adminService = adminService;
            _logger = logger;
            _dbConnections = dbConnections;
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(DatabaseConnections.SetupInformation))]
        public IActionResult Info()
        {
            try
            {
                var info = _dbConnections.SetupInfo;

                _logger.LogInformation($"{nameof(Info)}:\n{JsonConvert.SerializeObject(info)}");
                return Ok(info);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Info)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
         }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
            Policy = null, Roles = "sysadmin")]
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<GstUsrInfoAllDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
       public async Task<IActionResult> Seed(string nrAttractions = "1000", string nrAdresses = "100")
        {
            try
            {
                if (!int.TryParse(nrAttractions, out int countArgAtt) || !int.TryParse(nrAdresses, out int countArgAdd))
                    return BadRequest("Invalid input: nrAttractions and nrAdresses must be numbers.");

                if (countArgAtt < 1000 || countArgAdd < 100)
                    return BadRequest("Values too low: nrAttractions must be at least 1000, and nrAdresses must be at least 100.");

                _logger.LogInformation($"{nameof(Seed)}: {nameof(countArgAtt)}: {countArgAtt}, {nameof(countArgAdd)}: {countArgAdd}");
                var info = await _adminService.SeedAsync(countArgAtt, countArgAdd);
                return Ok(info);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Seed)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

         [HttpGet()]
        [ProducesResponseType(200, Type = typeof(UsrInfoDto))]
        [ProducesResponseType(400, Type = typeof(string))]
       public async Task<IActionResult> SeedUsers(string countUsr = "47", string countSupUsr = "2", string countSysAdmin = "1")
        {
            try
            {
                int _countUsr = int.Parse(countUsr);
                int _countSupUsr = int.Parse(countSupUsr);
                int _countSysAdmin = int.Parse(countSysAdmin);

                var sumAll = _countUsr + _countSupUsr + _countSysAdmin;

                if (sumAll < 50)
                return BadRequest("Value to low, you have to seed atleast 50 users");

                _logger.LogInformation($"{nameof(SeedUsers)}: {nameof(_countUsr)}: {_countUsr}, {nameof(_countSupUsr)}: {_countSupUsr}, {nameof(_countSysAdmin)}: {_countSysAdmin}");

                UsrInfoDto _info = await _adminService.SeedUsersAsync(_countUsr, _countSupUsr, _countSysAdmin);
                return Ok(_info);           
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}.{ex.InnerException?.Message}");
            }       
        }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
            Policy = null, Roles = "sysadmin")]
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<GstUsrInfoAllDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> RemoveSeed(string seeded = "true")
        {
            try
            {
                bool seededArg = bool.Parse(seeded);

                _logger.LogInformation($"{nameof(RemoveSeed)}: {nameof(seededArg)}: {seededArg}");
                var info = await _adminService.RemoveSeedAsync(seededArg);
                return Ok(info);        
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(RemoveSeed)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<LogMessage>))]
        public async Task<IActionResult> Log([FromServices] ILoggerProvider _loggerProvider)
        {
            //Note the way to get the LoggerProvider, not the logger from Services via DI
            if (_loggerProvider is InMemoryLoggerProvider cl)
            {
                return Ok(await cl.MessagesAsync);
            }
            return Ok("No messages in log");
        }
    }
}

