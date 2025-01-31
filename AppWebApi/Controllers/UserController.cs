using Models.DTO;
using Services;
using Microsoft.AspNetCore.Mvc;
using Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : Controller
{
    readonly IUserService _userService;
    readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet()]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IUser>))]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    public async Task<IActionResult> ReadItem(string id)
    {
        try
        {
            var idArg = Guid.Parse(id);
            var item = await _userService.ReadUserAsync(idArg);
            if (item?.Item == null) throw new ArgumentException($"User with id {id} does not exist");
            return Ok(item);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(ReadItem)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    [HttpPost()]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IUser>))]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    public async Task<IActionResult> CreateItem([FromBody] UserCuDto itemPost)
    {
        try
        {
            var item = await _userService.CreateUserAsync(itemPost);
            return Ok(item);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(CreateItem)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    [HttpPut()]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IUser>))]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    public async Task<IActionResult> UpdateItem([FromBody] UserCuDto itemPost)
    {
        try
        {
            var item = await _userService.UpdateUserAsync(itemPost);
            return Ok(item);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateItem)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete()]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IUser>))]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    public async Task<IActionResult> DeleteItem(string id)
    {
        try
        {
            var idArg = Guid.Parse(id);
            var item = await _userService.DeleteUserAsync(idArg);
            return Ok(item);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(DeleteItem)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }
}
