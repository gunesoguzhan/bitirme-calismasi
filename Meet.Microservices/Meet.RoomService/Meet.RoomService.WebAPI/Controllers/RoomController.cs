using System.Security.Claims;
using Meet.RoomService.WebAPI.Common;
using Meet.RoomService.WebAPI.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meet.RoomService.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RoomController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<RoomController> _logger;

    public RoomController(ApplicationDbContext dbContext, ILogger<RoomController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userIdString = User.FindFirstValue("userId");
        if (userIdString == null)
            return Unauthorized();
        try
        {
            var userId = Guid.Parse(userIdString);
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) return NotFound();
            var rooms = _dbContext.Rooms.Where(x => x.Users.Contains(user));
            return Ok(rooms.Select(x => x.AsDto()));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception caught: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}