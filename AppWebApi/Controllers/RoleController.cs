using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Models.DTO;
using Services;
using Models;
using System.Drawing;
using Microsoft.AspNetCore.Http.HttpResults;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
     [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : Controller
    {
        readonly IRoleService _roleService;
        readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _logger = logger;
            _roleService = roleService;
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(ResponsePageDto<IRole>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItems(string flat = "true", string pageNr = "0", string pageSize = "10")
        {
            try
            {
                bool flatArg = bool.Parse(flat);
                int pageNrArg = int.Parse(pageNr);
                int pageSizeArg = int.Parse(pageSize);

                // Lägg logger sen

                var resp = await _roleService.ReadRolesAsync( flatArg, pageNrArg, pageSizeArg);     
                return Ok(resp);  
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            

        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<IRole>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItem(string id = null, string flat = "false")
        {
            try
            {
                var idArg = Guid.Parse(id);
                bool flatArg = bool.Parse(flat);

                _logger.LogInformation($"{nameof(ReadItem)}: {nameof(idArg)}: {idArg}, {nameof(flatArg)}: {flatArg}");
                
                var item = await _roleService.ReadRoleAsync(idArg, flatArg);
                if (item?.Item == null) throw new ArgumentException ($"Item with id {id} does not exist");

                return Ok(item);         
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItem)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }   

        [HttpPut()]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<IRole>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> UpdateItem([FromBody] RoleCuDto itemPost)
         {
            try
            {
                _logger.LogInformation($"{nameof(UpdateItem)}: {nameof(itemPost)}: {itemPost}");
                var item = await _roleService.UpdateRoleAsync(itemPost);
                if (item?.Item == null) throw new ArgumentException ($"Item with id {item.Item.RoleId} does not exist"); 
                return Ok(item);


            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(UpdateItem)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}