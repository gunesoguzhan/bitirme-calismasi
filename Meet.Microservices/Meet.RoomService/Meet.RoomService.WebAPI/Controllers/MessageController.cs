using System.Security.Claims;
using Meet.RoomService.WebAPI.Common;
using Meet.RoomService.WebAPI.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meet.RoomService.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<MessageController> _logger;

    public MessageController(ApplicationDbContext dbContext, ILogger<MessageController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet("{roomId}")]
    public async Task<IActionResult> GetMessagesByRoomIdAsync(Guid roomId)
    {
        var userIdString = User.FindFirstValue("userId");
        if (userIdString == null)
            return Unauthorized();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) return NotFound();
            var room = _dbContext.Rooms
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == roomId && x.Users.Contains(user));
            if (room == null) return NotFound();
            var messages = room.Messages.Select(x => x.AsDto());
            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has been caught", ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}