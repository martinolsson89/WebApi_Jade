using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Models.DTO;
using Services;
using Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AttractionController : Controller
    {
        readonly IAttractionService _attractionService;
        readonly ILogger<AttractionController> _logger;

        public AttractionController(IAttractionService attractionService, ILogger<AttractionController> logger)
        {
            _attractionService = attractionService;
            _logger = logger;
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(List<IAttraction>))]
        public async Task<IActionResult> Read()
        {
            try
            {
                var attractions = await _attractionService.ReadAsync();

                _logger.LogInformation($"{nameof(Read)}");
                return Ok(attractions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Read)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IAttraction))]
        public async Task<IActionResult> ReadItem(string id)
        {
            try
            {
                Guid atId = Guid.Parse(id);
                var attraction = await _attractionService.ReadItemAsync(atId);

                _logger.LogInformation($"{nameof(Read)}");
                return Ok(attraction);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Read)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

