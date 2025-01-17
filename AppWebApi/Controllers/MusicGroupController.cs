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
    public class MusicGroupController : Controller
    {
        readonly IMusicGroupService _musicgroupService;
        readonly ILogger<MusicGroupController> _logger;

        public MusicGroupController(IMusicGroupService musicgroupService, ILogger<MusicGroupController> logger)
        {
            _musicgroupService = musicgroupService;
            _logger = logger;
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(List<IMusicGroup>))]
        public async Task<IActionResult> Read()
        {
            try
            {
                var musicGroups = await _musicgroupService.ReadAsync();

                _logger.LogInformation($"{nameof(Read)}");
                return Ok(musicGroups);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Read)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IMusicGroup))]
        public async Task<IActionResult> ReadItem(string id)
        {
            try
            {
                Guid mgId = Guid.Parse(id);
                var musicGroup = await _musicgroupService.ReadItemAsync(mgId);

                _logger.LogInformation($"{nameof(Read)}");
                return Ok(musicGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Read)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

