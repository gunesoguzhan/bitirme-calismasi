using System.Security.Claims;
using Meet.RoomService.WebAPI.Common;
using Meet.RoomService.WebAPI.DataAccess;
using Microsoft.AspNetCore.Mvc;
namespace Meet.RoomService.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
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
    public IActionResult Get()
    {
        var userIdString = User.FindFirstValue("userId");
        if (userIdString == null)
        {
            _logger.LogWarning("There is a problem with JWT.");
            return BadRequest();
        }
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
            return Ok(_dbContext.Rooms.Where(x => x.Users.Contains(userId)).Select(x => x.AsDto()));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}